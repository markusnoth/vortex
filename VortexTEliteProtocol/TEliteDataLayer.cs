// -------------------------------------------------------------------
// Project: Vortex TElite Protocol
//
// $HeadURL: http://svn/repos/casper/CASPer/trunk/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteDataLayer.cs $
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
using System.Runtime.InteropServices;
using System.Collections;
using VortexTEliteProtocol;

namespace VortexTEliteProtocol
{

    /// <summary>
    /// Structure class of TElite data frame
    /// </summary>
    public class TEliteDataFrame
    {

        #region Fields
        //**************************************************
        // Fields
        //**************************************************

        #region Private fields
        //**************************************************
        // Private fields
        //**************************************************

        /// <summary>
        /// TElite message data
        /// </summary>
        private TEliteMessage m_Message = null;

        /// <summary>
        /// TElite message terminator sequenze
        /// </summary>
        private byte[] m_Terminator = new byte[2] { 0xF8, 0x01 };

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
        /// Property to set/get TElite message data
        /// </summary>
        public TEliteMessage Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }

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
        /// Initializes a new instance of the TEliteDataFrame class.
        /// </summary>
        public TEliteDataFrame(byte[] dataFrame)
        {
            switch (this.GetMessageCode(dataFrame))
            {
                case (byte)TEliteMessage.MessageTypeCode.CommandResponse:
                    this.m_Message = new TEliteCommandResponse(this.GetMessageFrame(dataFrame)); break;
                case (byte)TEliteMessage.MessageTypeCode.FileBlock:
                    this.m_Message = new TEliteFileBlock(this.GetMessageFrame(dataFrame)); break;
                case (byte)TEliteMessage.MessageTypeCode.FileBrowse:
                    this.m_Message = new TEliteCommandResponse(this.GetMessageFrame(dataFrame), TEliteMessage.MessageTypeCode.FileBrowse); break;
                case (byte)TEliteMessage.MessageTypeCode.UserAccessResponse:
                    this.m_Message = new TEliteUserAccessResponse(this.GetMessageFrame(dataFrame)); break;
                case (byte)TEliteMessage.MessageTypeCode.PageRequest:
                    this.m_Message = new TElitePageRequest(this.GetMessageFrame(dataFrame)); break;
                case (byte)TEliteMessage.MessageTypeCode.PageWithCommandRow:
                    this.m_Message = new TElitePageWithCommandRow(this.GetMessageFrame(dataFrame)); break;
                case (byte)TEliteMessage.MessageTypeCode.ErrorResponse:
                    this.m_Message = new TEliteErrorResponse(this.GetMessageFrame(dataFrame)); break;
                default: this.m_Message = null; break;
            }
        }

        /// <summary>
        /// Initializes a new instance of the TEliteDataFrame class.
        /// </summary>
        public TEliteDataFrame(TEliteMessage message)
        {
            this.m_Message = message;
        }
        #endregion

        #region Destructor
        //**************************************************
        // Destructor
        //**************************************************

        /// <summary>
        /// default destructor of class
        /// </summary>
        ~TEliteDataFrame()
        {
        }
        #endregion

        #region Public Methods
        //**************************************************
        // Public Methods
        //**************************************************

        /// <summary>
        /// Converts data of a dataframe to a byte array
        /// </summary>
        /// <returns>byte array of dataframe</returns>
        public byte[] ToArray()
        {
            byte[] messageFrame = this.m_Message.ToArray();

            // count the needed trip characters on message frame
            int tripCount = 0;
            for (int i = 0; i < messageFrame.Length; i++)
            {
                if (messageFrame[i] == 0xF8)
                {
                    tripCount++;
                }
            }

            // initialize dataframe byte array
            byte[] dataFrame = new byte[messageFrame.Length + tripCount + 3];

            // copy message code
            dataFrame[0] = (byte)this.m_Message.Cmd;

            // copy message data
            int position = 1;
            for (int i = 0; i < messageFrame.Length; i++)
            {
                // insert trip character
                if (messageFrame[i] == 0xF8)
                {
                    dataFrame[position] = 0xF8;
                    position++;
                }
                dataFrame[position] = messageFrame[i];

                position++;
            }

            // set terminator sequence
            dataFrame[dataFrame.Length - 2] = m_Terminator[0];
            dataFrame[dataFrame.Length - 1] = m_Terminator[1];

            return dataFrame;
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
        /// Gets the command code of the message
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private byte GetMessageCode(byte[] array)
        {
            return array[0];
        }

        /// <summary>
        /// Gets the message data of data frame
        /// </summary>
        /// <param name="dataFrame"></param>
        /// <returns></returns>
        private byte[] GetMessageFrame(byte[] dataFrame)
        {
            // get count of present trip chars
            int tripCount = 0;
            for (int i = 1; i < dataFrame.Length - 2; i++)
            {
                if (dataFrame[i] == 0xF8)
                {
                    tripCount++;
                }
            }

            // initialize message frame byte array
            byte[] messageFrame = new byte[dataFrame.Length - tripCount - 3];

            int position = 0;
            for (int i = 1; i < (messageFrame.Length + 1); i++)
            {
                messageFrame[position] = dataFrame[i];

                // remove trip character
                if ((i + 1) < messageFrame.Length)
                {
                    if ((dataFrame[i] == 0xF8) && (dataFrame[i + 1] == 0xF8))
                    {
                        position++;
                    }
                }
                position++;
            }

            return messageFrame;
        }
        #endregion

        #endregion

    }

    /// <summary>
    /// Main class of TElite data layer
    /// </summary>
    public class TEliteDataLayer
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
        /// Initializes a new instance of the TEliteDataLayer class.
        /// </summary>
        public TEliteDataLayer()
        {
        }
        #endregion

        #region Destructor
        //**************************************************
        // Destructor
        //**************************************************

        /// <summary>
        /// default destructor of class
        /// </summary>
        ~TEliteDataLayer()
        {
        }
        #endregion

        #region Public Methods
        //**************************************************
        // Public Methods
        //**************************************************

        /// <summary>
        /// Method handle the request messages to Vortex
        /// </summary>
        /// <param name="message"></param>
        public void Request(TEliteMessage message)
        {
            TEliteDataFrame dataFrame = new TEliteDataFrame(message);

            if (Output != null)
            {
                Output(this, dataFrame);
            }
        }

        /// <summary>
        /// Method handle the response messages from Vortex
        /// </summary>
        /// <param name="data"></param>
        public void Response(byte[] data)
        {
            TEliteDataFrame dataFrame = new TEliteDataFrame(data);

            if (Input != null)
            {
                Input(this, dataFrame);
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

        #endregion

        #region Delegates
        //**************************************************
        // Delegates
        //**************************************************

        /// <summary>
        /// Delegate if data has added to the data layer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dataFrame"></param>
        public delegate void OutputEventHandler(object sender, TEliteDataFrame dataFrame);

        /// <summary>
        /// Delegate if data has geted from data layer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dataFrame"></param>
        public delegate void InputEventHandler(object sender, TEliteDataFrame dataFrame);

        /// <summary>
        /// Event-Handler if a data frame was sendet to Vortex.
        /// This event handler is not thread safe.
        /// </summary>
        public event OutputEventHandler Output;

        /// <summary>
        /// Event-handler if a data frame was received from Vortex.
        /// This event handler is not thread safe.
        /// </summary>
        public event InputEventHandler Input;

        #endregion

        #region Eventhandlers
        //**************************************************
        // Eventhandlers
        //**************************************************

        #endregion

        #endregion

    }

}
/* 
 * -------------------------------------------------------------------
 * History:
 * $Log: /CASPer/03_SW/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteDataLayer.cs $
 * 
 * 12    20.07.09 16:37 Not
 * file data also with cleanup available
 * 
 * 11    20.07.09 13:34 Not
 * command line responses in file format can be handled now
 * 
 * 10    16.07.09 14:41 Not
 * file block functionality (incomplete)
 * 
 * 9     2.07.09 15:35 san
 * 
 * 8     2.07.09 15:25 san
 * 
 * 7     23.06.09 14:45 san
 * 
 * 6     22.06.09 10:54 san
 * 
 * 5     19.06.09 15:01 san
 * 
 * 4     18.06.09 16:23 san
 * 
 * 3     18.06.09 14:58 san
 * 
 * 2     17.06.09 16:29 san
 * 
 * 1     17.06.09 15:06 san
 * -------------------------------------------------------------------
 */