// -------------------------------------------------------------------
// Project: Vortex TElite Protocol
//
// $HeadURL: http://svn/repos/casper/CASPer/trunk/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteTransportLayer.cs $
// $Author: har $
// $Date: 2011-06-14 11:17:47 +0200 (Di, 14 Jun 2011) $
// $Revision: 656 $
//
// Description:
//
// -------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

namespace VortexTEliteProtocol
{
    /// <summary>
    /// TEliteTransportLayer
    /// </summary>
    public class TEliteTransportLayer
    {

        #region Enumerations
        //**************************************************
        // Enumerations
        //**************************************************

        #endregion


        #region Constants
        //**************************************************
        // Constants
        //**************************************************
        /// <summary>
        /// VORTEX_DEF_PORT
        /// </summary>
        public const int VORTEX_DEF_PORT = 1025;
        /// <summary>
        /// RECEIVE_TIMEOUT
        /// </summary>
        public const int RECEIVE_TIMEOUT = 4; // seconds

        #endregion


        #region Fields
        //**************************************************
        // Fields
        //**************************************************

        #region Public fields
        //**************************************************
        // Public fields
        //**************************************************

        #endregion

        #region Protected fields
        //**************************************************
        // Protected fields
        //**************************************************

        #endregion

        #region Private fields
        //**************************************************
        // Private fields
        //**************************************************

        private ManualResetEvent m_ThreadEnd = new ManualResetEvent(false);

        /// <summary>
        /// TCP Client class reference
        /// </summary>
        private TcpClient m_TcpClient = null;

        /// <summary>
        /// Receive Thread of TCP Connection
        /// </summary>
        private Thread m_Thread = null;

        /// <summary>
        /// Member variable of Host port number
        /// </summary>
        private int m_Port = VORTEX_DEF_PORT;

        /// <summary>
        /// Member variable of Host address
        /// </summary>
        private string m_Host = string.Empty;

        #endregion

        #endregion


        #region Properties
        //**************************************************
        // Properties
        //**************************************************

        #region Public properties
        //**************************************************
        // Public properties
        //**************************************************

        /// <summary>
        /// Vortex Host port number
        /// </summary>
        public int Port
        {
            get { return m_Port; }
            set { m_Port = value; }
        }

        /// <summary>
        /// Vortex Host address (IP or Domain)
        /// </summary>
        public string Host
        {
            get { return m_Host; }
            set { m_Host = value; }
        }

        #endregion

        #region Protected properties
        //**************************************************
        // Protected properties
        //**************************************************

        #endregion

        #region Private properties
        //**************************************************
        // Private properties
        //**************************************************

        #endregion

        #endregion


        #region Methods
        //**************************************************
        // Methods
        //**************************************************

        #region Constructor
        //**************************************************
        // Constructor
        //**************************************************

        /// <summary>
        /// Initializes a new instance of the TEliteTransportLayer class.
        /// </summary>
        public TEliteTransportLayer()
        {
            m_TcpClient = new TcpClient();
        }
        #endregion

        #region Destructor
        //**************************************************
        // Destructor
        //**************************************************

        /// <summary>
        /// default destructor of class
        /// </summary>
        ~TEliteTransportLayer()
        {
            this.Disconnect();
        }
        #endregion

        #region Public Methods
        //**************************************************
        // Public Methods
        //**************************************************

        /// <summary>
        /// Connect to Vortex host
        /// </summary>
        /// <param name="host">host name or IP address</param>
        /// <param name="port">host port number</param>
        public void Connect(string host, int port)
        {
            m_Host = host;
            m_Port = port;

            this.Connect();
        }

        /// <summary>
        /// Connect to Vortex host
        /// </summary>
        public void Connect()
        {
            m_ThreadEnd.Reset();

            m_TcpClient = new TcpClient();

            m_Thread = new Thread(new ThreadStart(ReceiveData));
            m_Thread.Start();

            m_TcpClient.Connect(m_Host, m_Port);

            if (Connected != null)
            {
                Connected(this);
            }
        }

        /// <summary>
        /// Disconnect connection to Vortex
        /// </summary>
        public void Disconnect()
        {
            m_ThreadEnd.Set();

            if (m_Thread != null)
            {
                if (m_Thread.ThreadState == ThreadState.Running)
                {
                    m_Thread.Join();
                }
            }

            this.Close();
        }

        /// <summary>
        /// Send data to Vortex
        /// </summary>
        /// <param name="data">data as byte array stream</param>
        public void Send(byte[] data)
        {
            if (m_TcpClient != null)
            {
                try
                {
                    m_TcpClient.GetStream().Write(data, 0, data.Length);

                    if (Sended != null)
                    {
                        Sended(this, data);
                    }
                }
                catch (Exception ex)
                {
                    this.Disconnect();
                    throw ex;
                }
            }
        }
        #endregion

        #region Protected Methods
        //**************************************************
        // Protected Methods
        //**************************************************

        #endregion

        #region Privat Methods
        //**************************************************
        // Privat Methods
        //**************************************************

        /// <summary>
        /// Thread methode of receiving TCP/IP data
        /// </summary>
        private void ReceiveData()
        {
            try
            {
                while (!m_ThreadEnd.WaitOne(10, true))
                {
                    if ((m_TcpClient != null) && m_TcpClient.Connected)
                    {
                        if (m_TcpClient.GetStream().DataAvailable)
                        {
                            // buffer list for received data
                            List<byte[]> bufferList = new List<byte[]>();
                            bufferList.Clear();

                            // set DateTime to measure timeout
                            DateTime startTime = DateTime.UtcNow;

                            // to receive data from host it must wait until terminator sequence has received,
                            // because a TCP/IP package has a limited size (normally 512 bytes)
                            int totSize = 0;
                            int frameEnd = -1;
                            bool dataComplete = false;
                            do
                            {
                                // read data from TCP/IP stream
                                if (m_TcpClient.GetStream().DataAvailable)
                                {
                                    byte[] buffer = new byte[2048];
                                    int size = m_TcpClient.GetStream().Read(buffer, 0, buffer.Length);
                                    // check if data has received
                                    if (size > 0)
                                    {
                                        totSize += size;
                                        bufferList.Add(this.GetSubArray(buffer, 0, size));
                                    }
                                }

                                byte[] data = this.GetArrayFromArrayList(bufferList);

                                // check if Terminator sequenze is in data frame
                                frameEnd = this.GetFrameEndPos(data);

                                if (frameEnd >= 0)
                                {
                                    // end of frame received
                                    if (this.Received != null)
                                    {
                                        byte[] dataFrame = this.GetSubArray(data, 0, frameEnd + 2);
                                        // call Received event handler
                                        this.Received(this, dataFrame);
                                    }
                                    // check if more data of other frame has received
                                    if (frameEnd != (totSize - 2))
                                    {
                                        // save other frame data
                                        byte[] otherData = this.GetSubArray(data, frameEnd + 2, totSize);
                                        bufferList.Clear();
                                        totSize = totSize - (frameEnd + 2);
                                        bufferList.Add(otherData);
                                    }
                                    else
                                    {
                                        dataComplete = true;
                                    }
                                }
                            } while (!(dataComplete) && ((DateTime.UtcNow - startTime) < TimeSpan.FromSeconds(RECEIVE_TIMEOUT)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Close();
                throw ex;
            }
            finally
            {
                m_ThreadEnd.Reset();
            }
        }

        /// <summary>
        /// Gets the position of the end of the Frame.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private int GetFrameEndPos(byte[] buffer)
        {
            int pos = -1;

            for (int i = 0; i < buffer.Length-1; i++)
            {
                if ((buffer[i] == 0xF8) && (buffer[i + 1] == 0x01))
                {
                    pos = i;
                    break;
                }
            }

            return pos;
        }

        /// <summary>
        /// Gets a subarray from array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <returns></returns>
        private byte[] GetSubArray(byte[] array, int startPos, int endPos)
        {
            byte[] subarray = new byte[endPos - startPos];
            for (int i = 0; i < subarray.Length; i++)
            {
                subarray[i] = array[startPos + i];
            }
            return subarray;
        }

        /// <summary>
        /// Gets a byte array from ArrayList
        /// </summary>
        /// <param name="byteList"></param>
        /// <returns></returns>
        private byte[] GetArrayFromArrayList(List<byte[]> byteList)
        {
            // get total bytes in byte list
            int totSize = 0;
            foreach (byte[] data in byteList)
            {
                totSize += data.Length;
            }

            byte[] array = new byte[totSize];

            // copy byte list data to array
            int lastPos = 0;
            foreach (byte[] data in byteList)
            {
                data.CopyTo(array, lastPos);
                lastPos += data.Length;
            }

            return array;
        }

        /// <summary>
        /// Close TCP/IP Connection
        /// </summary>
        private void Close()
        {
            if (m_TcpClient != null)
            {
                m_TcpClient.Client.Close();
                m_TcpClient.Close();
                m_TcpClient = null;
            }

            if (Disconnected != null)
            {
                Disconnected(this);
            }
        }
        #endregion

        #region Eventhandlers
        //**************************************************
        // Eventhandlers
        //**************************************************

        /// <summary>
        /// Delegate if TCP/IP Connection has established
        /// </summary>
        /// <param name="sender"></param>
        public delegate void ConnectedHandler(object sender);

        /// <summary>
        /// Delegate if TCP/IP Connection has disconnected
        /// </summary>
        /// <param name="sender"></param>
        public delegate void DisconnectedHandler(object sender);

        /// <summary>
        /// Delegate if data has received from TCP/IP Connection stream
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        public delegate void ReceivedEventHandler(object sender, byte[] data);

        /// <summary>
        /// Delegate if data has sended to TCP/IP Connection stream
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        public delegate void SendedEventHandler(object sender, byte[] data);

        /// <summary>
        /// Event-Handler if TCP/IP Connection has established
        /// This event handler is not thread safe.
        /// </summary>
        public event ConnectedHandler Connected;

        /// <summary>
        /// Event-Handler if TCP/IP Connection has disconnected
        /// This event handler is not thread safe.
        /// </summary>
        public event DisconnectedHandler Disconnected;

        /// <summary>
        /// Event-Handler if data has received from TCP/IP Connection stream.
        /// This event handler is not thread safe.
        /// </summary>
        public event ReceivedEventHandler Received;

        /// <summary>
        /// Event-handler if data has sended to TCP/IP Connection stream.
        /// This event handler is not thread safe.
        /// </summary>
        public event SendedEventHandler Sended;

        #endregion

        #endregion

    }

}
/* 
 * -------------------------------------------------------------------
 * History:
 * $Log: /CASPer/03_SW/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteTransportLayer.cs $
 * 
 * 14    16.07.09 11:40 san
 * 
 * 13    15.07.09 14:15 san
 * Add new function: Get Page-Attribute EP1 from Vortex.
 * 
 * 12    6.07.09 9:57 san
 * Correction:
 * -Check connection and login state, before send commands.
 * -Decrease protocol timeouts on receive, answer and command.
 * 
 * 11    3.07.09 10:07 san
 * Correction on receive-thread (if size of data that was received is
 * zero) and end thread process.
 * 
 * 10    2.07.09 12:28 san
 * 
 * 9     1.07.09 9:45 san
 * 
 * 8     23.06.09 16:25 san
 * 
 * 7     23.06.09 16:06 san
 * 
 * 6     22.06.09 13:36 san
 * 
 * 5     22.06.09 10:54 san
 * 
 * 4     22.06.09 8:34 san
 * 
 * 3     19.06.09 15:01 san
 * 
 * 2     18.06.09 14:58 san
 * 
 * 1     17.06.09 15:06 san
 * -------------------------------------------------------------------
 */