using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using UnityEngine.UI;

public class HeartRateServer : MonoBehaviour
{
	public Text hrText;

    static private HeartRateServer s_singleton;

    private TcpListener m_ServerSocket;
    private TcpClient m_Client;

    private Dictionary<int, int> m_heartRatesCounts = null;
    private int m_baseHeartRate = 0;
    private int m_heartRate = 0;

    #region PROPERTIES
    public bool heartRateDefined
    {
        get { return m_heartRate > 0; }
    }

    public int currentHeartRate
    {
        get { return m_heartRate; }
    }

    public int baseHeartRate
    {
        get { return m_baseHeartRate; }
    }

    public bool baseHeartRateDefined
    {
        get { return m_baseHeartRate > 0; }
    }

    public int heartRateDelta
    {
        get { return m_heartRate - m_baseHeartRate; }
    }

    public float heartRateRatio
    {
        get
        {
            float tmp = m_heartRate;
            return m_heartRate / m_baseHeartRate;
        }
    }

    public float heartRateDeltaRatio
    {
        get
        {
            float delta = heartRateDelta;
            return delta / m_baseHeartRate;
        }
    }

    static public HeartRateServer singleton
    {
        get
        {
            return s_singleton;
        }
    }
    #endregion

    private void Awake()
    {
        s_singleton = this;
        ServerStart(1337);
    }

    private void Update()
    {
		hrText.text = m_heartRate.ToString();
		UpdateHeartRate();
        // TODO check validity
        //UpdateBaseHeartRate();
    }

    private void UpdateHeartRate()
    {
       
        if (m_Client == null)
        {
            return;
        }

        NetworkStream nStream = m_Client.GetStream();

        string data = null;
        byte[] bytes = new byte[256];

        while (nStream.DataAvailable)
        {
            int i = nStream.Read(bytes, 0, bytes.Length);
            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
        }

        if (data == null)
        {
            return;
        }

        // Check if data received is the mean value
        if (baseHeartRate == 0 && data.Split(':').Length > 1)
        {
            int tempHRM = 0;
            if (data.Split(':')[0].Equals("Mean value") && int.TryParse(data.Split(':')[1], out tempHRM))
            {
                m_baseHeartRate = tempHRM;
                return;
            }
        }
        int heartRateValue;
        if (ParseHeartRate(data, out heartRateValue))
        {
            m_heartRate = heartRateValue;
        }
    }

    private void UpdateBaseHeartRate()
    {
        if ((m_heartRatesCounts != null) && heartRateDefined)
        {
            if (m_heartRatesCounts.ContainsKey(m_heartRate))
            {
                m_heartRatesCounts[m_heartRate] = m_heartRatesCounts[m_heartRate] + 1;
            }
            else
            {
                m_heartRatesCounts.Add(m_heartRate, 1);
            }
        }
    }

    private bool ParseHeartRate(string data, out int heartRateValue)
    {
        int i;
        int lastDigitIndex = -1;
        for (i = 0; i < data.Length; i++)
        {
            char c = data[i];
            if ((c >= '0') && (c <= '9'))
            {
                lastDigitIndex = i;
            }
        }

        if (lastDigitIndex < 0)
        {
            heartRateValue = -1;
            return false;
        }

        for (i = lastDigitIndex; i >= 0; i--)
        {
            char c = data[i];
            if ((c < '0') || (c > '9'))
            {
                break;
            }
        }

        data = data.Substring(i + 1, lastDigitIndex - i);
        return int.TryParse(data, out heartRateValue);
    }

    private void ServerStart(int port)
    {
        if (m_ServerSocket == null)
        {
            try
            {
                m_ServerSocket = new TcpListener(IPAddress.Any, port);
                m_ServerSocket.Start();

                // Start asynch listening
                m_Client = m_ServerSocket.AcceptTcpClient();

                // Send connection confirmation
                NetworkStream outStream = m_Client.GetStream();
                byte[] msg = System.Text.ASCIIEncoding.ASCII.GetBytes("Connection done\n");
                
                outStream.Write(msg, 0, msg.Length);
                Debug.Log("msg :" + msg);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

    }

    [ContextMenu("StartGettingBaseHeartRate")]
    public void StartGettingBaseHeartRate()
    {
        m_heartRatesCounts = new Dictionary<int, int>();
    }

    [ContextMenu("StopGettingBaseHeartRate")]
    public void StopGettingBaseHeartRate()
    {
        List<int> heartRates = new List<int>(m_heartRatesCounts.Keys);
        int totalHeartRateCount = 0;
        m_baseHeartRate = 0;
        for (int i = 0; i < heartRates.Count; i++)
        {
            int heartRate = heartRates[i];
            int heartRateCount = m_heartRatesCounts[heartRate];
            totalHeartRateCount += heartRateCount;

            m_baseHeartRate += heartRate * heartRateCount;
        }

        m_baseHeartRate /= totalHeartRateCount;
        m_heartRatesCounts = null;
    }
}
