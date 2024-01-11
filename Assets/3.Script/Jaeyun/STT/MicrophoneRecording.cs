using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
using UnityEngine.Networking;
#endif

public class MicrophoneRecording : MonoBehaviour
{
    private string microphoneId = null;
    private AudioClip recording = null;
    private int recordingLength = 15; // recording �ִ� ��
    private int recordingHZ = 22050;

    private void Start()
    {
        microphoneId = Microphone.devices[0];
    }

    private void MicrophonePermission()
    { // Android ���� ��û
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            { 
                Permission.RequestUserPermission(Permission.Microphone);
            }
        }
    }
    #region MicrophoneRecoring
    public void StartRecording()
    { // Recording Button Click �� ȣ���ϴ� method
        MicrophonePermission();
        Debug.Log("Start Recording");
        recording = Microphone.Start(microphoneId, false, recordingLength, recordingHZ);
        StartCoroutine(StopRecording_Co());
        return;
    }

    public IEnumerator StopRecording_Co()
    {
        yield return new WaitForSeconds(7f); // 7�� ����
        if (Microphone.IsRecording(microphoneId))
        {
            Microphone.End(microphoneId);

            Debug.Log("Stop Recording");
            if (recording == null)
            {
                Debug.LogError("Nothing Recorded");
            }

            byte[] byteData = GetByteFromAudioClip(recording); // audio clip to byte array
            StartCoroutine(PostVoice_Co("Kor", byteData));
        }
    }
    #endregion
    private byte[] GetByteFromAudioClip(AudioClip audioClip)
    { // audio clip�� �������� �ʰ� �ٷ� byte�� ��ȯ�ϱ�
        float[] samples = new float[audioClip.samples];
        audioClip.GetData(samples, 0);

        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);

        int length = samples.Length;
        writer.Write(length);

        foreach(float sample in samples)
        {
            writer.Write(sample);
        }

        byte[] bytes = stream.ToArray();
        return bytes;
    }

    [SerializeField]
    public class VoiceRecognize
    { // �޾ƿ� ���� �����ϰ� �����ϱ� ���� json ����
        public string text;
    }

    #region API STT
    private string client_id = "tzzjets5nx";
    private string client_api_key = "ovtg4WKrK1R1nw6FExiinMK3Nafk4IEWuTmdw9fw";

    private IEnumerator PostVoice_Co(string lang, byte[] data)
    {
        string url = $"https://naveropenapi.apigw.ntruss.com/recog/v1/stt?lang=";
        url += lang; // ��� �ڵ� ( Kor, Jpn, Eng, Chn )
        // Request
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Headers.Add("X-NCP-APIGW-API-KEY-ID", $"{client_id}");
        request.Headers.Add("X-NCP-APIGW-API-KEY", $"{client_api_key}");
        request.Method = "POST";
        request.ContentType = "application/octet-stream";
        request.ContentLength = data.Length;
        using (Stream requestStream = request.GetRequestStream())
        {
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
        }


        // Response
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        
        yield return request.GetResponse(); // request �� response ���� ������ ���

        Stream stream = response.GetResponseStream();
        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        string text = reader.ReadToEnd();
        stream.Close();
        response.Close();
        reader.Close();

        Debug.Log(text);
    }
    #endregion
}
