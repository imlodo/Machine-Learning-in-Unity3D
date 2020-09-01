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
        public Text testo;

        public void Initialize()
        {
            sizeConfig = GetComponent<SizeConfig>();
            sizeConfig.RaiseResizeEvent += OnScreenResize;
            Size size = sizeConfig.Initialize();

            texture = new Texture2D(size.Image.x, size.Image.y, TextureFormat.RGB24, false);
            cam = GameObject.FindObjectOfType<Cam>();
            cam.Initialize(ref texture, size);

            monitor = GameObject.FindObjectOfType<Monitor>();
            monitor.Initialize(size, LabelColors.CreateFromJSON(Resources.Load<TextAsset>("LabelColors").text));

            clientManager = new ClientManager(ref texture);
            clientManager.RaiseDetectionEvent += OnDetection;
        }

        void Start()
        {
            if(!stop)
                Initialize();
        }

        void Update()
        {
            if(!stop)
               clientManager.Update();
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
                        SelectPhoto.selectPhoto();
                    }
                    testo.text = countPersonDetect + "";
                }
            }
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
    }
}