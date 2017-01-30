// -------------------------------------------------------------------
// Project: Vortex TElite Protocol
//
// $HeadURL: http://svn/repos/casper/CASPer/trunk/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TElitePageResponse.cs $
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

namespace VortexTEliteProtocol
{
    /// <summary>
    /// TElitePageResponse
    /// </summary>
    public class TElitePageResponse : TEliteMessage
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

        /// <summary>
        /// Member variable of EP1File class
        /// </summary>
        private EP1File m_EP1File = null;

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
        /// Initializes a new instance of the TElitePageResponse class.
        /// </summary>
        /// <param name="ep1File">EP1 file to send</param>
        public TElitePageResponse(EP1File ep1File)
        {
            this.m_MessageOrigin = MessageOriginEnum.Client;
            this.m_Cmd = MessageTypeCode.PageResponse;

            m_EP1File = ep1File;
        }

        #endregion

        #region Destructor
        //**************************************************
        // Destructor
        //**************************************************

        /// <summary>
        /// default destructor of class
        /// </summary>
        ~TElitePageResponse()
        {
            m_EP1File = null;
        }
        #endregion

        #region Public Methods
        //**************************************************
        // Public Methods
        //**************************************************

        /// <summary>
        /// Converts PageResponse message to a byte array
        /// </summary>
        /// <returns></returns>
        public override byte[] ToArray()
        {
            byte[] ep1Data = new byte[1025];
            int line = 0;
            int blanks = 0;

            // copy row zero
            ep1Data[0] = 0;
            m_EP1File.GetByteHeaderRow().CopyTo(ep1Data, 1);
            // copy line 1 to 23
            for (byte i = 1; i <= 23; i++)
            {
                // blank lines should not transfered to vortex
                if (this.IsBlank(m_EP1File.GetByteLine(i, 40)))
                {
                    blanks++;
                }
                else
                {
                    line++;
                    ep1Data[(line * 41)] = i;
                    m_EP1File.GetByteLine(i, 40).CopyTo(ep1Data, (line * 41) + 1);
                }
            }
            // copy line 24
            ep1Data[((line + 1) * 41)] = 24;
            m_EP1File.GetByteCommandRow().CopyTo(ep1Data, ((line + 1) * 41) + 1);

            // trim ep1 data to a new array
            byte[] pageResponse = new byte[(1025 - (blanks * 41))];
            for (int i = 0; i < pageResponse.Length; i++)
            {
                pageResponse[i] = ep1Data[i];
            }

            return pageResponse;
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
        /// Checks, if a TXT-Line is a blank line.
        /// </summary>
        /// <param name="line">TXT-Line</param>
        /// <returns>true=is a blank line</returns>
        private bool IsBlank(byte[] line)
        {
            bool blank = true;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] != 0x20)
                {
                    blank = false;
                }
            }

            return blank;
        }
        #endregion

        #region Eventhandlers
        //**************************************************
        // Eventhandlers
        //**************************************************

        #endregion

        #region Web-Methods
        //**************************************************
        // Web-Methods
        //**************************************************

        #endregion

        #endregion

    }

}
/* 
 * -------------------------------------------------------------------
 * History:
 * $Log: /CASPer/03_SW/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TElitePageResponse.cs $
 * 
 * 3     9.11.09 10:13 san
 * Correction: No transfer of blank lines to Vortex. PageResponse modified
 * (for Vortex Merge function).
 * 
 * 2     23.06.09 14:45 san
 * 
 * 1     22.06.09 13:35 san
 * -------------------------------------------------------------------
 */