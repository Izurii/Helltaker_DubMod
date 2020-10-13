using UnityEngine;

namespace Helltaker_DubMod
{
    public class Main
    {
        public static void StaticInitMethod()
        {
            new Thread(() =>
            {
                Thread.Sleep(5000); // 5 second sleep as initialization occurs *really* early

                GameObject someObject = new GameObject();
                someObject.AddComponent<SomeComponent>(); // MonoBehaviour
                UnityEngine.Object.DontDestroyOnLoad(someObject);
                using (StreamWriter writetext = new StreamWriter("write.txt"))
                {
                    writetext.WriteLine("writing in text file");
                }

            }).Start();
        }
    }
}