// -------------------------------------------------------------------
// Project: Vortex TElite Protocol
//
// $HeadURL: http://svn/repos/casper/CASPer/trunk/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteProtocol.cs $
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
using System.Threading;

namespace VortexTEliteProtocol
{
    /// <summary>
    /// TEliteProtocol
    /// </summary>
    public class TEliteProtocol
    {

        #region Enumerations
        //**************************************************
        // Enumerations
        //**************************************************
        /// <summary>
        /// ProtocolStateEnum
        /// </summary>
        public enum ProtocolStateEnum
        {
            /// <summary>
            /// None
            /// </summary>
            None = 0,
            /// <summary>
            /// Connecting
            /// </summary>
            Connecting,
            /// <summary>
            /// Connected
            /// </summary>
            Connected,
            /// <summary>
            /// Disconnected
            /// </summary>
            Disconnected,
            /// <summary>
            /// Login
            /// </summary>
            Login,
            /// <summary>
            /// PasswordExpected
            /// </summary>
            PasswordExpected,
            /// <summary>
            /// UserAccessExpected
            /// </summary>
            UserAccessExpected,
            /// <summary>
            /// LoggedIn
            /// </summary>
            LoggedIn,
            /// <summary>
            /// Logout
            /// </summary>
            Logout,
            /// <summary>
            /// LoggedOut
            /// </summary>
            LoggedOut,
            /// <summary>
            /// CommandLine
            /// </summary>
            CommandLine,
            /// <summary>
            /// PageUploadRequest
            /// </summary>
            PageUploadRequest,
            /// <summary>
            /// PageUploadResponse
            /// </summary>
            PageUploadResponse,
            /// <summary>
            /// PageDownloadRequest
            /// </summary>
            PageDownloadRequest,
            /// <summary>
            /// PageDownloadResponse
            /// </summary>
            PageDownloadResponse
        };

        #endregion


        #region Constants
        //**************************************************
        // Constants
        //**************************************************

        /// <summary>
        /// Max wait timeout for command transfers
        /// </summary>
        private const int MAX_COMMAND_TIMEOUT = 10000; //milliseconds

        /// <summary>
        /// Max wait timeout for protocol handling
        /// </summary>
        private const int MAX_ANSWER_WAIT = 6000; //milliseconds

        #endregion

        #region Fields
        //**************************************************
        // Fields
        //**************************************************

        #region Private fields
        //**************************************************
        // Private fields
        //**************************************************

        /// <summary>
        /// TElite protocol data layer class
        /// </summary>
        private TEliteDataLayer m_DataLayer = null;

        /// <summary>
        /// TElite protocol transport layer class
        /// </summary>
        private TEliteTransportLayer m_TransportLayer = null;

        /// <summary>
        /// Member variable of username
        /// </summary>
        private string m_Username = string.Empty;

        /// <summary>
        /// Member variable of password
        /// </summary>
        private string m_Password = string.Empty;

        /// <summary>
        /// Member variable of protocolstate
        /// </summary>
        private ProtocolStateEnum m_ProtocolState = ProtocolStateEnum.None;

        /// <summary>
        /// Member variable if client has connected to Vortex
        /// </summary>
        private bool m_Connected = false;

        /// <summary>
        /// Member variable if user has logged in to Vortex
        /// </summary>
        private bool m_LoggedIn = false;

        /// <summary>
        /// Member variable Name of the Host the protocol connects to
        /// </summary>
        private string m_HostName;

        /// <summary>
        /// Member variable if max Vortex terminal has reached
        /// </summary>
        private bool m_NoMoreTerminals = false;

        /// <summary>
        /// Member variable if user account or terminal is disabled
        /// </summary>
        private bool m_UserDisabled = false;

        /// <summary>
        /// Protocol semaphore (execute only one command each)
        /// </summary>
        private Semaphore m_Semaphore = null;

        /// <summary>
        /// Protocol command wait events (signaling answer of vortex)
        /// </summary>
        private ManualResetEvent m_ConnectedEvent = null;
        private ManualResetEvent m_LoginEvent = null;
        private ManualResetEvent m_LogoutEvent = null;
        private ManualResetEvent m_CommandEvent = null;
        private ManualResetEvent m_UploadEvent = null;
        private ManualResetEvent m_DownloadEvent = null;

        /// <summary>
        /// Data Queues
        /// </summary>
        private Queue<EP1File> m_DownloadQueue = null;
        private EP1File m_UploadFile = null;
        private Queue<TEliteCommandResponse> m_CommandQueue = null;
        private Queue<byte[]> m_FileRecordQueue = null;

        /// <summary>
        /// Member variable to remeber last outgoing command line
        /// </summary>
        private TEliteDataFrame m_LastCommandLine = null;

        /// <summary>
        /// Member variable to remeber unfinished FileRecord data
        /// </summary>
        private byte[] m_IncompleteFileRecord = null;

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
        /// Gets the datalayer object
        /// </summary>
        public TEliteDataLayer DataLayer
        {
            get { return m_DataLayer; }
        }

        /// <summary>
        /// Gets state of client connection to vortex
        /// </summary>
        public bool Connected
        {
            get { return m_Connected; }
        }

        /// <summary>
        /// Gets state of user login
        /// </summary>
        public bool LoggedIn
        {
            get { return m_LoggedIn; }
        }

        /// <summary>
        /// Contains the HostName of the protocol
        /// </summary>
        public string HostName 
        {
            get { return m_HostName; }
            set { m_HostName = value; }
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
        /// Initializes a new instance of the VortexTEliteProtocol class.
        /// </summary>
        public TEliteProtocol()
        {
            // initialize protocol semaphore
            m_Semaphore = new Semaphore(0, 1);
            m_Semaphore.Release();

            // initialize event signaling objects
            m_ConnectedEvent = new ManualResetEvent(false);
            m_LoginEvent = new ManualResetEvent(false);
            m_LogoutEvent = new ManualResetEvent(false);
            m_CommandEvent = new ManualResetEvent(false);
            m_UploadEvent = new ManualResetEvent(false);
            m_DownloadEvent = new ManualResetEvent(false);

            // initialize data queues
            m_DownloadQueue = new Queue<EP1File>();
            m_DownloadQueue.Clear();
            m_CommandQueue = new Queue<TEliteCommandResponse>();
            m_CommandQueue.Clear();
            m_FileRecordQueue = new Queue<byte[]>();
            m_FileRecordQueue.Clear();

            // initialize transport layer object
            this.m_TransportLayer = new TEliteTransportLayer();
            this.m_TransportLayer.Disconnected += new TEliteTransportLayer.DisconnectedHandler(OnDisconnect);
            this.m_TransportLayer.Received += new TEliteTransportLayer.ReceivedEventHandler(OnReceived);

            // initialize data layer object
            this.m_DataLayer = new TEliteDataLayer();
            this.m_DataLayer.Output += new TEliteDataLayer.OutputEventHandler(OnDataOutput);
            this.m_DataLayer.Input += new TEliteDataLayer.InputEventHandler(OnDataInput);
        }
        #endregion

        #region Destructor
        //**************************************************
        // Destructor
        //**************************************************

        /// <summary>
        /// default destructor of class
        /// </summary>
        ~TEliteProtocol()
        {
            m_TransportLayer.Disconnect();

            m_ConnectedEvent.Close();
            m_LoginEvent.Close();
            m_LogoutEvent.Close();
            m_CommandEvent.Close();
            m_UploadEvent.Close();
            m_DownloadEvent.Close();

            m_Semaphore.Close();
        }
        #endregion

        #region Public Methods
        //**************************************************
        // Public Methods
        //**************************************************

        /// <summary>
        /// Connect to Vortex
        /// </summary>
        /// <param name="host">host as string IP or Name</param>
        /// <param name="port">Portnumber</param>
        public void Connect(string host, int port)
        {
            if (!this.Connected)
            {
                bool success = false;
                try
                {
                    if (m_Semaphore.WaitOne(MAX_COMMAND_TIMEOUT))
                    {
                        m_NoMoreTerminals = false;
                        m_ConnectedEvent.Reset();

                        m_ProtocolState = ProtocolStateEnum.Connecting;
                        m_TransportLayer.Connect(host, port);

                        if (m_ConnectedEvent.WaitOne(MAX_ANSWER_WAIT))
                        {
                            // Check if connection was not rejected from Vortex host
                            if (!m_NoMoreTerminals)
                            {
                                success = true;
                            }
                        }
                    }
                }
                finally
                {
                    m_Semaphore.Release();
                }
                if (!success)
                {
                    this.Disconnect();

                    if (m_NoMoreTerminals)
                    {
                        throw new Exception(string.Format("No more Vortex terminals available! Connection to Vortex host {0} on port {1} failed!", host, port));
                    }
                    else
                    {
                        throw new Exception(string.Format("Timeout on TElite protocol elabsed! Connection to Vortex host {0} on port {1} failed!", host, port));
                    }
                }
            }
        }

        /// <summary>
        /// Disconnect connection to Vortex
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (m_Semaphore.WaitOne(MAX_COMMAND_TIMEOUT))
                {
                    try
                    {
                        m_ProtocolState = ProtocolStateEnum.Disconnected;
                        m_TransportLayer.Disconnect();
                    }
                    finally
                    {
                        m_Connected = false;
                    }
                }
            }
            finally
            {
                m_Semaphore.Release();
            }
        }

        /// <summary>
        /// Login Command to Vortex
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password as plaintext string</param>
        public void Login(string username, string password)
        {
            if (this.Connected)
            {
                bool success = false;
                m_UserDisabled = false;
                try
                {
                    if (m_Semaphore.WaitOne(MAX_COMMAND_TIMEOUT))
                    {
                        m_LoginEvent.Reset();

                        m_ProtocolState = ProtocolStateEnum.Login;

                        m_Username = username;
                        m_Password = password;

                        TEliteCommandLine message = new TEliteCommandLine();
                        message.Login(username);

                        m_DataLayer.Request(message);

                        if (m_LoginEvent.WaitOne(MAX_ANSWER_WAIT))
                        {
                            if (!m_UserDisabled)
                            {
                                success = true;
                            }
                        }
                    }
                }
                finally
                {
                    m_Semaphore.Release();
                }
                if (!success)
                {
                    if (m_UserDisabled)
                    {
                        throw new Exception(string.Format("User account or terminal disabled! Login to Vortex with username {0} failed!", username));
                    }
                    else
                    {
                        throw new Exception(string.Format("Timeout on TElite protocol elabsed! Login to Vortex with username {0} failed!", username));
                    }
                }
            }
        }

        /// <summary>
        /// Logout Command ro Vortex
        /// </summary>
        public void Logout()
        {
            if (this.Connected && this.LoggedIn)
            {
                bool success = false;
                try
                {
                    if (m_Semaphore.WaitOne(MAX_COMMAND_TIMEOUT))
                    {
                        m_LogoutEvent.Reset();

                        m_ProtocolState = ProtocolStateEnum.Logout;

                        TEliteCommandLine message = new TEliteCommandLine();
                        message.Logout();

                        m_DataLayer.Request(message);

                        success = m_LogoutEvent.WaitOne(MAX_ANSWER_WAIT);
                    }
                }
                finally
                {
                    m_Semaphore.Release();
                }
                if (!success)
                {
                    throw new Exception("Timeout on TElite protocol elabsed! Logout from Vortex failed!");
                }
            }
        }

        /// <summary>
        /// Send command line to Vortex
        /// </summary>
        /// <param name="commandLine">Command line string</param>
        /// <returns>command response object</returns>
        public TEliteCommandResponse SendCommandLine(string commandLine)
        {
            TEliteCommandResponse command = null;
            if (this.Connected)
            {
                try
                {
                    if (m_Semaphore.WaitOne(MAX_COMMAND_TIMEOUT))
                    {
                        m_CommandEvent.Reset();

                        m_ProtocolState = ProtocolStateEnum.CommandLine;

                        TEliteCommandLine message = new TEliteCommandLine();
                        message.SetCommandLine(commandLine);

                        m_DataLayer.Request(message);

                        if (m_CommandEvent.WaitOne(MAX_ANSWER_WAIT))
                        {
                            // regular command response
                            if (m_CommandQueue.Count > 0)
                            {
                                command = m_CommandQueue.Dequeue();
                            }
                            // file block as command response
                            if (m_FileRecordQueue.Count > 0)
                            {
                                command = new TEliteCommandResponse();
                                command.FileData = m_FileRecordQueue.ToArray();
                            }
                        }
                    }
                }
                finally
                {
                    m_Semaphore.Release();
                }
            }
            return command;
        }

        /// <summary>
        /// Upload an EP1 file to Vortex with filename
        /// </summary>
        /// <param name="fileName">Filename with path</param>
        /// <returns>true=success</returns>
        public bool UploadEP1(string fileName)
        {
            EP1File ep1File = new EP1File();
            ep1File.ReadFile(fileName);

            return this.UploadEP1(ep1File);
        }

        /// <summary>
        /// Upload an EP1 file to Vortex with row 24 command
        /// </summary>
        /// <param name="ep1File">EP1 file</param>
        /// <returns>true=success</returns>
        public bool UploadEP1(EP1File ep1File)
        {
            bool success = false;
            if (this.Connected && this.LoggedIn)
            {
                try
                {
                    if (m_Semaphore.WaitOne(MAX_COMMAND_TIMEOUT))
                    {
                        m_UploadEvent.Reset();

                        m_ProtocolState = ProtocolStateEnum.PageUploadRequest;
                        m_UploadFile = ep1File;

                        TEliteCommandLine message = new TEliteCommandLine();
                        message.Upload(ep1File.GetCommandRow());
                        m_DataLayer.Request(message);

                        if (m_UploadEvent.WaitOne(MAX_ANSWER_WAIT))
                        {
                            if (m_CommandQueue.Count > 0)
                            {
                                if (m_CommandQueue.Dequeue().ColorResult != TEliteCommandResponse.ColorResultEnum.Error)
                                {
                                    success = true;
                                }
                            }
                            else
                            {
                                // on truncate page, no page was requested
                                success = true;
                            }
                        }
                    }
                }
                finally
                {
                    m_Semaphore.Release();
                }
            }
            return success;
        }

        /// <summary>
        /// Upload an EP1 file to Vortex
        /// </summary>
        /// <param name="magazin">Magazin number</param>
        /// <param name="set">Set number</param>
        /// <param name="page">Page number</param>
        /// <param name="ep1File">EP1 file</param>
        /// <returns>true=success</returns>
        public bool UploadEP1(int magazin, int set, int page, EP1File ep1File)
        {
            bool success = false;
            if (this.Connected && this.LoggedIn)
            {
                try
                {
                    if (m_Semaphore.WaitOne(MAX_COMMAND_TIMEOUT))
                    {
                        m_UploadEvent.Reset();

                        m_ProtocolState = ProtocolStateEnum.PageUploadRequest;
                        m_UploadFile = ep1File;

                        TEliteCommandLine message = new TEliteCommandLine();
                        message.Upload(magazin, set, page);
                        m_DataLayer.Request(message);

                        if (m_UploadEvent.WaitOne(MAX_ANSWER_WAIT))
                        {
                            if (m_CommandQueue.Count > 0)
                            {
                                if (m_CommandQueue.Dequeue().ColorResult != TEliteCommandResponse.ColorResultEnum.Error)
                                {
                                    success = true;
                                }
                            }
                            else
                            {
                                // on truncate page, no page was requested
                                success = true;
                            }
                        }
                    }
                }
                finally
                {
                    m_Semaphore.Release();
                }
            }
            return success;
        }

        /// <summary>
        /// Download an EP1 file from Vortex
        /// </summary>
        /// <param name="magazin">Magazin number</param>
        /// <param name="set">Set number</param>
        /// <param name="page">Page number</param>
        /// <returns>EP1file object</returns>
        public EP1File DownloadEP1(int magazin, int set, int page)
        {
            EP1File ep1File = null;
            if (this.Connected && this.LoggedIn)
            {
                try
                {
                    if (m_Semaphore.WaitOne(MAX_COMMAND_TIMEOUT))
                    {
                        m_DownloadEvent.Reset();

                        m_ProtocolState = ProtocolStateEnum.PageDownloadRequest;

                        TEliteCommandLine message = new TEliteCommandLine();
                        message.Download(magazin, set, page);
                        m_DataLayer.Request(message);

                        if (m_DownloadEvent.WaitOne(MAX_ANSWER_WAIT))
                        {
                            if (m_DownloadQueue.Count > 0)
                            {
                                ep1File = m_DownloadQueue.Dequeue();
                            }
                        }
                    }
                }
                finally
                {
                    m_Semaphore.Release();
                }
            }
            return ep1File;
        }

        /// <summary>
        /// Attributes of Magazin from Vortex
        /// </summary>
        /// <param name="magazin"></param>
        /// <returns></returns>
        public VortexAttributes Attributes(int magazin)
        {
            return this.Attributes(magazin, -1, -1);
        }

        /// <summary>
        /// Attributes of Set from Vortex
        /// </summary>
        /// <param name="magazin"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        public VortexAttributes Attributes(int magazin, int set)
        {
            return this.Attributes(magazin, set, -1);
        }

        /// <summary>
        /// Attributes of Page from Vortex
        /// </summary>
        /// <param name="magazin">Magazin number</param>
        /// <param name="set">Set number</param>
        /// <param name="page">Page number</param>
        /// <returns>EP1file object</returns>
        public VortexAttributes Attributes(int magazin, int set, int page)
        {
            VortexAttributes attributes = null;
            if (this.Connected && this.LoggedIn)
            {
                try
                {
                    if (m_Semaphore.WaitOne(MAX_COMMAND_TIMEOUT))
                    {
                        m_DownloadEvent.Reset();

                        m_ProtocolState = ProtocolStateEnum.PageDownloadRequest;

                        TEliteCommandLine message = new TEliteCommandLine();

                        message.Attributes(magazin, set, page);
                        m_DataLayer.Request(message);

                        if (m_DownloadEvent.WaitOne(MAX_ANSWER_WAIT))
                        {
                            if (m_DownloadQueue.Count > 0)
                            {
                                EP1File ep1File = m_DownloadQueue.Dequeue();
                                if (ep1File != null)
                                {
                                    attributes = new VortexAttributes(ep1File);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    m_Semaphore.Release();
                }
            }
            return attributes;
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
        /// Handles the protocol output data to Vortex
        /// </summary>
        /// <param name="dataFrame"></param>
        private void HandleOutputData(TEliteDataFrame dataFrame)
        {
            if (dataFrame.Message != null)
            {
                if (dataFrame.Message.Cmd == TEliteMessage.MessageTypeCode.CommandLine)
                {
                    m_LastCommandLine = dataFrame;
                }
                m_TransportLayer.Send(dataFrame.ToArray());
            }
        }

        /// <summary>
        /// Handles the protocol input data from Vortex
        /// </summary>
        /// <param name="dataFrame"></param>
        private void HandleInputData(TEliteDataFrame dataFrame)
        {
            if (dataFrame.Message != null)
            {
                // Call Error Command Response Event
                if (dataFrame.Message.Cmd == TEliteMessage.MessageTypeCode.CommandResponse)
                {
                    TEliteCommandResponse command = (TEliteCommandResponse)dataFrame.Message;
                    if (command.ColorResult == TEliteCommandResponse.ColorResultEnum.Error)
                    {
                        if (this.Error != null)
                        {
                            string message = command.CommandLine;
                            ThreadSafe.Invoke(this.Error, new object[] { this, message, m_LastCommandLine });
                        }
                    }
                }
                if (dataFrame.Message.Cmd == TEliteMessage.MessageTypeCode.ErrorResponse)
                {
                    TEliteErrorResponse command = (TEliteErrorResponse)dataFrame.Message;

                    if (m_ProtocolState == ProtocolStateEnum.Connecting)
                    {
                        m_NoMoreTerminals = true;
                        m_ConnectedEvent.Set();
                    }

                    if (this.Error != null)
                    {
                        string message = command.CommandLine;
                        ThreadSafe.Invoke(this.Error, new object[] { this, message, m_LastCommandLine });
                    }
                }

                // Handle protocol process
                switch (m_ProtocolState)
                {
                    case ProtocolStateEnum.Connecting:
                        {
                            if (dataFrame.Message.Cmd == TEliteMessage.MessageTypeCode.CommandResponse)
                            {
                                TEliteCommandResponse command = (TEliteCommandResponse)dataFrame.Message;
                                if (command.CommandLine.Contains("Connection made to"))
                                {
                                    m_Connected = true;
                                    m_ProtocolState = ProtocolStateEnum.Connected;

                                    m_ConnectedEvent.Set();
                                }
                            }
                        } break;
                    case ProtocolStateEnum.Login:
                        {
                            if (dataFrame.Message.Cmd == TEliteMessage.MessageTypeCode.CommandResponse)
                            {
                                TEliteCommandResponse command = (TEliteCommandResponse)dataFrame.Message;
                                if (command.CommandLine.Contains("Enter password :"))
                                {
                                    m_ProtocolState = ProtocolStateEnum.PasswordExpected;

                                    TEliteCommandLine message = new TEliteCommandLine();
                                    message.Password(m_Password);

                                    m_DataLayer.Request(message);
                                }
                                if (command.CommandLine.Contains("User or terminal disabled"))
                                {
                                    m_ProtocolState = ProtocolStateEnum.None;
                                    m_UserDisabled = true;
                                    m_LoginEvent.Set();
                                }
                            }
                        } break;
                    case ProtocolStateEnum.PasswordExpected:
                        {
                            if (dataFrame.Message.Cmd == TEliteMessage.MessageTypeCode.UserAccessResponse)
                            {
                                m_ProtocolState = ProtocolStateEnum.UserAccessExpected;
                            }
                        } break;
                    case ProtocolStateEnum.UserAccessExpected:
                        {
                            if (dataFrame.Message.Cmd == TEliteMessage.MessageTypeCode.CommandResponse)
                            {
                                TEliteCommandResponse command = (TEliteCommandResponse)dataFrame.Message;
                                if (command.CommandLine.Contains("Welcome to VORTEX"))
                                {
                                    m_LoggedIn = true;
                                    m_ProtocolState = ProtocolStateEnum.LoggedIn;

                                    m_LoginEvent.Set();
                                }
                            }
                        } break;
                    case ProtocolStateEnum.Logout:
                        {
                            if (dataFrame.Message.Cmd == TEliteMessage.MessageTypeCode.CommandResponse)
                            {
                                TEliteCommandResponse command = (TEliteCommandResponse)dataFrame.Message;
                                if (command.CommandLine.Contains("Good Bye"))
                                {
                                    m_LoggedIn = false;
                                    m_ProtocolState = ProtocolStateEnum.LoggedOut;

                                    m_LogoutEvent.Set();
                                }
                            }
                        } break;
                    case ProtocolStateEnum.CommandLine:
                        {
                            // handling for regular command line response
                            if (dataFrame.Message.Cmd == TEliteMessage.MessageTypeCode.CommandResponse)
                            {
                                m_ProtocolState = ProtocolStateEnum.None;

                                TEliteCommandResponse command = (TEliteCommandResponse)dataFrame.Message;

                                m_CommandQueue.Enqueue(command);

                                m_CommandEvent.Set();
                            }
                            // handling if command line returns a file
                            if (dataFrame.Message.Cmd == TEliteMessage.MessageTypeCode.FileBlock)
                            {
                                TEliteFileBlock fileBlock = ((TEliteFileBlock)dataFrame.Message);

                                int fileRecordLength = 0;
                                for (int i = 2; i < fileBlock.Data.Length; ) // first 2 bytes are file block length
                                {
                                    if (m_IncompleteFileRecord == null)
                                    {
                                        fileRecordLength = fileBlock.Data[i] + (fileBlock.Data[i + 1] << 8);
                                        if (fileBlock.Data.Length >= i + fileRecordLength + 2)
                                        {
                                            // if complete FileRecord is available, copy the data to the queue
                                            byte[] fileRecordData = new byte[fileRecordLength];
                                            Array.Copy(fileBlock.Data, i + 2, fileRecordData, 0, fileRecordData.Length);
                                            m_FileRecordQueue.Enqueue(fileRecordData);
                                        }
                                        else
                                        {
                                            // if only part of the FileRecord is available, copy the fragment to the cache-variable
                                            m_IncompleteFileRecord = new byte[fileBlock.Data.Length - i];
                                            Array.Copy(fileBlock.Data, i, m_IncompleteFileRecord, 0, m_IncompleteFileRecord.Length);
                                        }
                                        // advance cursor position
                                        i += fileRecordLength + 2;
                                    }
                                    else
                                    {
                                        fileRecordLength = m_IncompleteFileRecord[i] + (m_IncompleteFileRecord[i + 1] << 8);
                                        int newDataLength = fileRecordLength - (m_IncompleteFileRecord.Length - 2);
                                        if (m_IncompleteFileRecord.Length - 2 + fileBlock.Data.Length >= fileRecordLength)
                                        {
                                            byte[] fileRecordData = new byte[fileRecordLength];
                                            Array.Copy(m_IncompleteFileRecord, 2, fileRecordData, 0, fileRecordLength - newDataLength);
                                            Array.Copy(fileBlock.Data, 0, fileRecordData, m_IncompleteFileRecord.Length - 1, newDataLength);
                                            m_FileRecordQueue.Enqueue(fileRecordData);
                                            m_IncompleteFileRecord = null;
                                        }
                                        else
                                        {
                                            byte[] newIncompleteFileRecord = new byte[m_IncompleteFileRecord.Length + fileBlock.Data.Length];
                                            m_IncompleteFileRecord.CopyTo(newIncompleteFileRecord, 0);
                                            fileBlock.Data.CopyTo(newIncompleteFileRecord, newIncompleteFileRecord.Length);
                                            m_IncompleteFileRecord = newIncompleteFileRecord;
                                        }
                                        // advance cursor position
                                        i += newDataLength;
                                    }

                                    // odd file record length have a filler byte afterwards
                                    if (fileRecordLength % 2 == 1) i++;
                                }
                            }
                            // returning of file content is finished
                            if (dataFrame.Message.Cmd == TEliteMessage.MessageTypeCode.FileBrowse)
                            {
                                // if was last package reset protocol state enum to none and release data
                                m_ProtocolState = ProtocolStateEnum.None;
                                m_CommandQueue.Enqueue((TEliteCommandResponse)dataFrame.Message);
                                m_CommandEvent.Set();
                            }
                        } break;
                    case ProtocolStateEnum.PageUploadRequest:
                        {
                            if (dataFrame.Message.Cmd == TEliteMessage.MessageTypeCode.PageRequest)
                            {
                                m_ProtocolState = ProtocolStateEnum.PageUploadResponse;

                                TElitePageResponse message = new TElitePageResponse(m_UploadFile);
                                m_DataLayer.Request(message);
                            }
                            else
                            {
                                m_UploadEvent.Set();
                            }
                        } break;
                    case ProtocolStateEnum.PageUploadResponse:
                        {
                            if (dataFrame.Message.Cmd == TEliteMessage.MessageTypeCode.CommandResponse)
                            {
                                m_ProtocolState = ProtocolStateEnum.None;

                                TEliteCommandResponse command = (TEliteCommandResponse)dataFrame.Message;
                                m_CommandQueue.Enqueue(command);

                                m_UploadEvent.Set();
                            }
                        } break;
                    case ProtocolStateEnum.PageDownloadRequest:
                        {
                            if (dataFrame.Message.Cmd == TEliteMessage.MessageTypeCode.PageWithCommandRow)
                            {
                                m_ProtocolState = ProtocolStateEnum.PageDownloadResponse;

                                EP1File ep1File = ((TElitePageWithCommandRow)dataFrame.Message).ToEP1File();

                                m_DownloadQueue.Enqueue(ep1File);

                                m_DownloadEvent.Set();
                            }
                        } break;
                }

                // Handle Errors of Command Response Message
                if (dataFrame.Message.Cmd == TEliteMessage.MessageTypeCode.CommandResponse)
                {
                    TEliteCommandResponse command = (TEliteCommandResponse)dataFrame.Message;
                    if (command.ColorResult == TEliteCommandResponse.ColorResultEnum.Error)
                    {
                        if (m_ProtocolState == ProtocolStateEnum.PageDownloadRequest)
                        {
                            m_DownloadEvent.Set();
                        }
                    }
                }
            }
        }

        #endregion

        #region Delegates
        //**************************************************
        // Delegates
        //**************************************************

        /// <summary>
        /// Delegate for transmission data to Vortex
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dataFrame"></param>
        public delegate void OutputEventHandler(object sender, TEliteDataFrame dataFrame);

        /// <summary>
        /// Delegate for received data from Vortex
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dataFrame"></param>
        public delegate void InputEventHandler(object sender, TEliteDataFrame dataFrame);

        /// <summary>
        /// Delegate for error commands from Vortex
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <param name="dataFrame"></param>
        public delegate void ErrorHandler(object sender, string message, TEliteDataFrame dataFrame);

        /// <summary>
        /// Event-Handler if a data frame was sendet to Vortex.
        /// This event handler is thread safe.
        /// </summary>
        public event OutputEventHandler Output;

        /// <summary>
        /// Event-handler if a data frame was received from Vortex.
        /// This event handler is thread safe.
        /// </summary>
        public event InputEventHandler Input;

        /// <summary>
        /// Event-handler if an Error was received from Vortex.
        /// This event handler is thread safe.
        /// </summary>
        public event ErrorHandler Error;

        #endregion

        #region Eventhandlers
        //**************************************************
        // Eventhandlers
        //**************************************************

        // I/O Handlers from Data-Layer

        /// <summary>
        /// Event-Handler if TCP/IP connection has disconnected
        /// </summary>
        /// <param name="sender"></param>
        private void OnDisconnect(object sender)
        {
            m_Connected = false;
        }

        /// <summary>
        /// Event-Handler if data has sended from the data layer to Vortex
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dataFrame"></param>
        private void OnDataOutput(object sender, TEliteDataFrame dataFrame)
        {
            this.HandleOutputData(dataFrame);

            if (this.Output != null)
            {
                ThreadSafe.Invoke(this.Output, new object[] { this, dataFrame });
            }
        }

        /// <summary>
        /// Event-Handler if data has received the data layer from Vortex.
        /// The event triggers, if the dataframe was created from data layer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dataFrame"></param>
        private void OnDataInput(object sender, TEliteDataFrame dataFrame)
        {
            if (this.Input != null)
            {
                ThreadSafe.Invoke(this.Input, new object[] { this, dataFrame });
            }

            this.HandleInputData(dataFrame);
        }

        // I/O Handlers from Transport-Layer

        /// <summary>
        /// Event-Handler if data from transport layer has received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        private void OnReceived(object sender, byte[] data)
        {
            m_DataLayer.Response(data);
        }
        #endregion

        #endregion

    }

}
/* 
 * -------------------------------------------------------------------
 * History:
 * $Log: /CASPer/03_SW/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteProtocol.cs $
 * 
 * 36    16.10.09 12:57 san
 * BugFix: Report success on truncate command.
 * 
 * 35    15.10.09 16:54 san
 * BugFix: Error on truncate page, page was not deleted in this case from
 * the file queue.
 * 
 * 34    20.07.09 16:37 Not
 * file data also with cleanup available
 * 
 * 33    20.07.09 13:34 Not
 * command line responses in file format can be handled now
 * 
 * 32    16.07.09 16:27 san
 * 
 * 31    16.07.09 14:41 Not
 * file block functionality (incomplete)
 * 
 * 30    16.07.09 11:40 san
 * 
 * 29    15.07.09 14:15 san
 * Add new function: Get Page-Attribute EP1 from Vortex.
 * 
 * 28    6.07.09 9:57 san
 * Correction:
 * -Check connection and login state, before send commands.
 * -Decrease protocol timeouts on receive, answer and command.
 * 
 * 27    3.07.09 15:36 san
 * 
 * 26    3.07.09 14:49 san
 * 
 * 25    3.07.09 10:07 san
 * Correction on receive-thread (if size of data that was received is
 * zero) and end thread process.
 * 
 * 24    3.07.09 7:52 san
 * 
 * 23    2.07.09 15:55 san
 * 
 * 22    2.07.09 15:35 san
 * 
 * 21    2.07.09 15:25 san
 * 
 * 20    2.07.09 14:26 san
 * 
 * 19    2.07.09 14:26 san
 * 
 * 18    2.07.09 13:59 san
 * 
 * 17    2.07.09 11:27 san
 * 
 * 16    2.07.09 10:02 san
 * 
 * 15    1.07.09 16:10 san
 * 
 * 14    1.07.09 15:11 san
 * Refactor TElite Protcol code to synchron mode.
 * 
 * 13    1.07.09 14:03 san
 * 
 * 12    1.07.09 9:45 san
 * 
 * 11    30.06.09 11:15 san
 * 
 * 10    23.06.09 16:06 san
 * 
 * 9     23.06.09 14:45 san
 * 
 * 8     22.06.09 16:24 san
 * 
 * 7     22.06.09 13:35 san
 * 
 * 6     22.06.09 10:54 san
 * 
 * 5     22.06.09 8:34 san
 * 
 * 4     19.06.09 15:01 san
 * 
 * 3     18.06.09 14:58 san
 * 
 * 2     17.06.09 16:29 san
 * 
 * 1     17.06.09 15:06 san
 * -------------------------------------------------------------------
 */