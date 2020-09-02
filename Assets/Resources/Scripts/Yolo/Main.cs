using System.Threading;
using UnityEngine;
using UnityEngine.UI;

// NOTE: Comment out BindService method in YoloServiceGrpc.cs, lines 91-94

namespace Yolo
{
    public class Main : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 1f)]
        float confidenceThreshold = 0;
        ClientManager clientManager;
        SizeConfig sizeConfig;
        Texture2D texture;
        Monitor monitor;
        Cam cam;
        private Texture2D[] texturesDetected = new Texture2D[10];
        public GameObject go;
        private int countPersonDetect = 0;
        private bool stop = false;
        public Text testoNumeroRiscontri;
        public GameObject initialFrame;
        public GameObject box;
        public Text topText;
        public Text bottomTextStatic;
        public Text bottomTextDinamic;
        public Canvas canvas;
        public GameObject areaImage;

        public void Initialize()
        {
            stop = false;
            countPersonDetect = 0;
            texturesDetected = new Texture2D[10];
            sizeConfig = GetComponent<SizeConfig>();
            sizeConfig.RaiseResizeEvent += OnScreenResize;
            Size size = sizeConfig.Initialize();

            texture = new Texture2D(size.Image.x, size.Image.y, TextureFormat.RGB24, false);
            cam = GameObject.FindObjectOfType<Cam>();
            cam.Initialize(ref texture, size);

            monitor = GameObject.FindObjectOfType<Monitor>();
            monitor.Initialize(size, LabelColors.CreateFromJSON(Resources.Load<TextAsset>("LabelColors").text));

            WebCamTexture backCam = LoadCamera.loadCamera();
            initialFrame.GetComponent<Renderer>().material.mainTexture = backCam;

            Texture textNoBorder = areaImage.GetComponent<Renderer>().material.mainTexture;
            clientManager = new ClientManager(ref texture, ref textNoBorder);
            clientManager.RaiseDetectionEvent += OnDetection;
        }

        void Start()
        {
            if(!stop)
                Initialize();
        }

        void Update()
        {
            if (!stop)
            {
                clientManager.Update();
            }
            else
                go.GetComponent<Renderer>().material.mainTexture = texturesDetected[countPersonDetect - 1];
        }

        void OnDetection(object sender, DetectionEventArgs e)
        {
            foreach(YoloItem y in e.Result.ToList(confidenceThreshold))
            {
                Debug.Log(y.Type+" "+y.Confidence);

                if (y.Type == "person" && y.Confidence*100 > 60)
                {
                    Texture2D detected = (Texture2D) sender;
                    texturesDetected[countPersonDetect] = detected;
                    //Debug.Log("Person Detected");
                    countPersonDetect += 1;
                    if (countPersonDetect == 10) 
                    {
                        stop = true;
                        SelectPhoto.Initializate(this, texturesDetected);
                        SelectPhoto.selectPhoto();
                        HiddenObject("initialFrame");
                        HiddenObject("box");
                        HiddenObject("bottomTextStatic");
                        HiddenObject("bottomTextDinamic");
                        ChangeText("topText", "Clicca sull'immagine che ti rappresenta meglio:");
                    }
                    testoNumeroRiscontri.text = countPersonDetect + "";
                }
            }
            if(!stop)
             monitor.UpdateLabels(e.Result.ToList(confidenceThreshold));
        }

        void OnScreenResize(object sender, ResizeEventArgs e)
        {
            texture.Resize(e.Size.Image.x, e.Size.Image.y);
            monitor.SetSize(e.Size);
            cam.SetSize(e.Size);
        }

        void OnApplicationQuit()
        {
            sizeConfig.RaiseResizeEvent -= OnScreenResize;
            clientManager.RaiseDetectionEvent -= OnDetection;
            clientManager.Dispose();
        }

        public void HiddenObject(string object_name)
        {
            switch(object_name)
            {
                case "initialFrame": initialFrame.SetActive(false); break;
                case "box": box.SetActive(false);break;
                case "bottomTextStatic":bottomTextStatic.enabled = false; break;
                case "bottomTextDinamic":bottomTextDinamic.enabled = false; break;
                default: break;
            }
        }

        public void ShowObject(string object_name)
        {
            switch (object_name)
            {
                case "initialFrame": initialFrame.SetActive(true); break;
                case "box": box.SetActive(true); break;
                case "bottomTextStatic": bottomTextStatic.enabled = true; break;
                case "bottomTextDinamic": bottomTextDinamic.enabled = true; break;
                default: break;
            }
        }

        public void ChangeText(string object_name, string text)
        {
            switch(object_name)
            {

                case "topText": topText.text = text; break;
                default: break;
            }
        }
    }
}