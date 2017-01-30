// -------------------------------------------------------------------
// Project: Vortex TElite Protocol
//
// $HeadURL: http://svn/repos/casper/CASPer/trunk/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/VortexAttributesValues.cs $
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
    /// Baseclass of Attributes values
    /// </summary>
    public class AttributesValues
    {
        #region Fields
        //**************************************************
        // Fields
        //**************************************************

        #region Private fields
        //**************************************************
        // Private fields
        //**************************************************

        private string m_Title = string.Empty;
        private int m_Magazine = -1;
        private int m_Set = -1;
        private int m_Page = -1;

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
        /// Title
        /// </summary>
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }
        /// <summary>
        /// Magazine
        /// </summary>
        public int Magazine
        {
            get { return m_Magazine; }
            set { m_Magazine = value; }
        }
        /// <summary>
        /// Set
        /// </summary>
        public int Set
        {
            get { return m_Set; }
            set { m_Set = value; }
        }
        /// <summary>
        /// Page
        /// </summary>
        public int Page
        {
            get { return m_Page; }
            set { m_Page = value; }
        }
        #endregion

        #endregion

        #region Methods
        //**************************************************
        // Methods
        //**************************************************

        #region Public Methods
        //**************************************************
        // Public Methods
        //**************************************************
        
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
        /// Gets a string value of the EP1-File line
        /// </summary>
        /// <param name="ep1File"></param>
        /// <param name="lineNumber"></param>
        /// <param name="startPos"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected string GetString(EP1File ep1File, int lineNumber, int startPos, int length)
        {
            return System.Text.Encoding.UTF8.GetString(this.GetByteLine(ep1File, lineNumber, startPos, length)).Trim();
        }

        /// <summary>
        /// Gets a integer value of the EP1-File line
        /// </summary>
        /// <param name="ep1File"></param>
        /// <param name="lineNumber"></param>
        /// <param name="startPos"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected int GetInteger(EP1File ep1File, int lineNumber, int startPos, int length)
        {
            int value = -1;
            try
            {
                value = int.Parse(System.Text.Encoding.UTF8.GetString(this.GetByteLine(ep1File, lineNumber, startPos, length)).Trim());
            }
            catch
            {
            }
            return value;
        }

        /// <summary>
        /// Gets a bool value of the EP1-File line
        /// </summary>
        /// <param name="ep1File"></param>
        /// <param name="lineNumber"></param>
        /// <param name="startPos"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected bool GetBool(EP1File ep1File, int lineNumber, int startPos, int length)
        {
            bool value = false;

            if (this.GetString(ep1File, lineNumber, startPos, length) == "Y")
            {
                value = true;
            }

            return value;
        }

        /// <summary>
        /// Gets a byte line of EP1 file without control chars
        /// (replaced with space)
        /// </summary>
        /// <param name="ep1File"></param>
        /// <param name="lineNumber"></param>
        /// <param name="startPos"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected byte[] GetByteLine(EP1File ep1File, int lineNumber, int startPos, int length)
        {
            byte[] line = ep1File.GetByteLine(lineNumber, startPos, length);

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] < 0x1F)
                {
                    // replace control char with space character
                    line[i] = 0x20;
                }
            }
            return line;
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// Magazine attributes values
    /// </summary>
    public class MagazineAttributes : AttributesValues
    {
        #region Fields
        //**************************************************
        // Fields
        //**************************************************

        #region Private fields
        //**************************************************
        // Private fields
        //**************************************************

        private string m_DefaultAccess = string.Empty;
        private string m_Language = string.Empty;
        private int m_ReadSpeed = 0;
        private string m_Access = string.Empty;
        private string m_Owner = string.Empty;
        private string m_LastBackup = string.Empty;
        private int m_MaxPages = 0;
        private int m_PagesInUse = 0;

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
        /// DefaultAccess
        /// </summary>
        public string DefaultAccess
        {
            get { return m_DefaultAccess; }
            set { m_DefaultAccess = value; }
        }
        /// <summary>
        /// Language
        /// </summary>
        public string Language
        {
            get { return m_Language; }
            set { m_Language = value; }
        }
        /// <summary>
        /// ReadSpeed
        /// </summary>
        public int ReadSpeed
        {
            get { return m_ReadSpeed; }
            set { m_ReadSpeed = value; }
        }
        /// <summary>
        /// Access
        /// </summary>
        public string Access
        {
            get { return m_Access; }
            set { m_Access = value; }
        }
        /// <summary>
        /// Owner
        /// </summary>
        public string Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }
        /// <summary>
        /// LastBackup
        /// </summary>
        public string LastBackup
        {
            get { return m_LastBackup; }
            set { m_LastBackup = value; }
        }
        /// <summary>
        /// MaxPages
        /// </summary>
        public int MaxPages
        {
            get { return m_MaxPages; }
            set { m_MaxPages = value; }
        }
        /// <summary>
        /// PagesInUse
        /// </summary>
        public int PagesInUse
        {
            get { return m_PagesInUse; }
            set { m_PagesInUse = value; }
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
        /// Initializes a new instance of the VortexAttributes class.
        /// </summary>
        public MagazineAttributes(EP1File ep1File)
        {
            this.Parse(ep1File);
        }
        #endregion

        #region Privat Methods
        //**************************************************
        // Privat Methods
        //**************************************************

        /// <summary>
        /// Parse EP1-File and set properties
        /// </summary>
        /// <param name="ep1File"></param>
        private void Parse(EP1File ep1File)
        {
            // set magazine number
            string pageLine = this.GetString(ep1File, 1, 27, 4).Trim();
            this.Magazine = int.Parse(pageLine);

            // set Set properties
            this.Title = this.GetString(ep1File, 2, 27, 14);
            this.DefaultAccess = this.GetString(ep1File, 6, 10, 31);
            this.Language = this.GetString(ep1File, 7, 12, 5);
            this.ReadSpeed = this.GetInteger(ep1File, 9, 14, 27);
            this.Access = this.GetString(ep1File, 19, 10, 31);
            this.Owner = this.GetString(ep1File, 20, 16, 25);
            this.LastBackup = this.GetString(ep1File, 21, 15, 26);
            this.MaxPages = this.GetInteger(ep1File, 22, 15, 26);
            this.PagesInUse = this.GetInteger(ep1File, 23, 15, 26);
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Set attributes values
    /// </summary>
    public class SetAttributes : AttributesValues
    {
        #region Fields
        //**************************************************
        // Fields
        //**************************************************

        #region Private fields
        //**************************************************
        // Private fields
        //**************************************************

        private bool m_DefaultPageIncluded = false;
        private bool m_DefaultPageArchive = false;
        private string m_DefaultPageAutosequence = string.Empty;
        private string m_DefaultPageTimestamp = string.Empty;
        private string m_DefaultPageCopyProtect = string.Empty;
        private string m_DefaultPageIPProtect = string.Empty;
        private int m_DefaultPageRetransmitInterval = 0;
        private bool m_Included = false;
        private bool m_Archive = false;
        private bool m_InSeq = false;
        private bool m_Rush = false;
        private string m_TopsPrompt = string.Empty;
        private bool m_TopsClass = false;
        private string m_TopsSelect = string.Empty;
        private string m_Access = string.Empty;
        private string m_Owner = string.Empty;
        private int m_Pages = 0;

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
        /// DefaultPageIncluded
        /// </summary>
        public bool DefaultPageIncluded
        {
            get { return m_DefaultPageIncluded; }
            set { m_DefaultPageIncluded = value; }
        }
        /// <summary>
        /// DefaultPageArchive
        /// </summary>
        public bool DefaultPageArchive
        {
            get { return m_DefaultPageArchive; }
            set { m_DefaultPageArchive = value; }
        }
        /// <summary>
        /// DefaultPageAutosequence
        /// </summary>
        public string DefaultPageAutosequence
        {
            get { return m_DefaultPageAutosequence; }
            set { m_DefaultPageAutosequence = value; }
        }
        /// <summary>
        /// DefaultPageTimestamp
        /// </summary>
        public string DefaultPageTimestamp
        {
            get { return m_DefaultPageTimestamp; }
            set { m_DefaultPageTimestamp = value; }
        }
        /// <summary>
        /// DefaultPageCopyProtect
        /// </summary>
        public string DefaultPageCopyProtect
        {
            get { return m_DefaultPageCopyProtect; }
            set { m_DefaultPageCopyProtect = value; }
        }
        /// <summary>
        /// DefaultPageIPProtect
        /// </summary>
        public string DefaultPageIPProtect
        {
            get { return m_DefaultPageIPProtect; }
            set { m_DefaultPageIPProtect = value; }
        }
        /// <summary>
        /// DefaultPageRetransmitInterval
        /// </summary>
        public int DefaultPageRetransmitInterval
        {
            get { return m_DefaultPageRetransmitInterval; }
            set { m_DefaultPageRetransmitInterval = value; }
        }
        /// <summary>
        /// Included
        /// </summary>
        public bool Included
        {
            get { return m_Included; }
            set { m_Included = value; }
        }
        /// <summary>
        /// Archive
        /// </summary>
        public bool Archive
        {
            get { return m_Archive; }
            set { m_Archive = value; }
        }
        /// <summary>
        /// InSeq
        /// </summary>
        public bool InSeq
        {
            get { return m_InSeq; }
            set { m_InSeq = value; }
        }
        /// <summary>
        /// Rush
        /// </summary>
        public bool Rush
        {
            get { return m_Rush; }
            set { m_Rush = value; }
        }
        /// <summary>
        /// TopsPrompt
        /// </summary>
        public string TopsPrompt
        {
            get { return m_TopsPrompt; }
            set { m_TopsPrompt = value; }
        }
        /// <summary>
        /// TopsClass
        /// </summary>
        public bool TopsClass
        {
            get { return m_TopsClass; }
            set { m_TopsClass = value; }
        }
        /// <summary>
        /// TopsSelect
        /// </summary>
        public string TopsSelect
        {
            get { return m_TopsSelect; }
            set { m_TopsSelect = value; }
        }

        ///<summary>
        /// Access
        ///</summary>
        public string Access
        {
            get { return m_Access; }
            set { m_Access = value; }
        }
        /// <summary>
        /// Owner
        /// </summary>
        public string Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }
        /// <summary>
        /// Pages
        /// </summary>
        public int Pages
        {
            get { return m_Pages; }
            set { m_Pages = value; }
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
        /// Initializes a new instance of the VortexAttributes class.
        /// </summary>
        public SetAttributes(EP1File ep1File)
        {
            this.Parse(ep1File);
        }
        #endregion

        #region Privat Methods
        //**************************************************
        // Privat Methods
        //**************************************************

        /// <summary>
        /// Parse EP1-File and set properties
        /// </summary>
        /// <param name="ep1File"></param>
        private void Parse(EP1File ep1File)
        {
            // set magazine & set number
            string pageLine = this.GetString(ep1File, 1, 25, 8);
            this.Magazine = int.Parse(pageLine.Trim().Split(' ')[0]);
            this.Set = int.Parse(pageLine.Trim().Split(' ')[1]);

            // set Set properties
            this.Title = this.GetString(ep1File, 2, 24, 17);
            this.DefaultPageIncluded = this.GetBool(ep1File, 5, 13, 1);
            this.DefaultPageArchive = this.GetBool(ep1File, 6, 13, 1);
            this.DefaultPageAutosequence = this.GetString(ep1File, 7, 18, 9);
            this.DefaultPageTimestamp = this.GetString(ep1File, 8, 18, 9);
            this.DefaultPageCopyProtect = this.GetString(ep1File, 9, 18, 9);
            this.DefaultPageIPProtect = this.GetString(ep1File, 10, 13, 27);
            this.DefaultPageRetransmitInterval = this.GetInteger(ep1File, 11, 25, 16);
            this.TopsPrompt = this.GetString(ep1File, 15, 12, 29);
            this.TopsClass = this.GetBool(ep1File, 16, 12, 1);
            this.TopsSelect = this.GetString(ep1File, 17, 12, 29);
            this.Access = this.GetString(ep1File, 19, 10, 31);
            this.Owner = this.GetString(ep1File, 20, 10, 31);
            this.Pages = this.GetInteger(ep1File, 21, 10, 10);

            // set Set flags
            this.Included = this.GetBool(ep1File, 5, 36, 1);
            this.Archive = this.GetBool(ep1File, 6, 36, 1);
            this.InSeq = this.GetBool(ep1File, 7, 36, 1);
            this.Rush = this.GetBool(ep1File, 8, 36, 1);
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Page attributtes values
    /// </summary>
    public class PageAttributes : AttributesValues
    {
        #region Fields
        //**************************************************
        // Fields
        //**************************************************

        #region Private fields
        //**************************************************
        // Private fields
        //**************************************************

        private string m_Autosequence = string.Empty;
        private string m_Timestamp = string.Empty;
        private int m_SelectCount = 0;
        private int m_Frequency = 0;
        private string m_Alias = string.Empty;
        private string m_Subcode = string.Empty;
        private string m_Autosource = string.Empty;
        private string m_Autobatch = string.Empty;
        private string m_CopyProtect = string.Empty;
        private string m_IPProtect = string.Empty;
        private string m_Protect = string.Empty;
        private string m_Advert = string.Empty;
        private string m_Access = string.Empty;
        private string m_Owner = string.Empty;
        private string m_Language = string.Empty;
        private DateTime m_LastUpdate = DateTime.MinValue;
        private string m_LastCommand = string.Empty;
        private bool m_Included = false;
        private bool m_Clear = false;
        private bool m_Update = false;
        private bool m_NewsFlash = false;
        private bool m_Subtitle = false;

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
        /// Autosequence
        /// </summary>
        public string Autosequence
        {
            get { return m_Autosequence; }
            set { m_Autosequence = value; }
        }
        /// <summary>
        /// Timestamp
        /// </summary>
        public string Timestamp
        {
            get { return m_Timestamp; }
            set { m_Timestamp = value; }
        }
        /// <summary>
        /// SelectCount
        /// </summary>
        public int SelectCount
        {
            get { return m_SelectCount; }
            set { m_SelectCount = value; }
        }
        /// <summary>
        /// Frequency
        /// </summary>
        public int Frequency
        {
            get { return m_Frequency; }
            set { m_Frequency = value; }
        }
        /// <summary>
        /// Alias
        /// </summary>
        public string Alias
        {
            get { return m_Alias; }
            set { m_Alias = value; }
        }
        /// <summary>
        /// Subcode
        /// </summary>
        public string Subcode
        {
            get { return m_Subcode; }
            set { m_Subcode = value; }
        }
        /// <summary>
        /// Autosource
        /// </summary>
        public string Autosource
        {
            get { return m_Autosource; }
            set { m_Autosource = value; }
        }
        /// <summary>
        /// Autobatch
        /// </summary>
        public string Autobatch
        {
            get { return m_Autobatch; }
            set { m_Autobatch = value; }
        }
        /// <summary>
        /// CopyProtect
        /// </summary>
        public string CopyProtect
        {
            get { return m_CopyProtect; }
            set { m_CopyProtect = value; }
        }
        /// <summary>
        /// IPProtect
        /// </summary>
        public string IPProtect
        {
            get { return m_IPProtect; }
            set { m_IPProtect = value; }
        }
        /// <summary>
        /// Protect
        /// </summary>
        public string Protect
        {
            get { return m_Protect; }
            set { m_Protect = value; }
        }
        /// <summary>
        /// Advert
        /// </summary>
        public string Advert
        {
            get { return m_Advert; }
            set { m_Advert = value; }
        }
        /// <summary>
        /// Access
        /// </summary>
        public string Access
        {
            get { return m_Access; }
            set { m_Access = value; }
        }
        /// <summary>
        /// Owner
        /// </summary>
        public string Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }
        /// <summary>
        /// Language
        /// </summary>
        public string Language
        {
            get { return m_Language; }
            set { m_Language = value; }
        }
        /// <summary>
        /// LastUpdate
        /// </summary>
        public DateTime LastUpdate
        {
            get { return m_LastUpdate; }
            set { m_LastUpdate = value; }
        }
        /// <summary>
        /// LastCommand
        /// </summary>
        public string LastCommand
        {
            get { return m_LastCommand; }
            set { m_LastCommand = value; }
        }
        /// <summary>
        /// Included
        /// </summary>
        public bool Included
        {
            get { return m_Included; }
            set { m_Included = value; }
        }
        private bool m_Archive = false;
        /// <summary>
        /// Archive
        /// </summary>
        public bool Archive
        {
            get { return m_Archive; }
            set { m_Archive = value; }
        }
        /// <summary>
        /// Clear
        /// </summary>
        public bool Clear
        {
            get { return m_Clear; }
            set { m_Clear = value; }
        }
        /// <summary>
        /// Update
        /// </summary>
        public bool Update
        {
            get { return m_Update; }
            set { m_Update = value; }
        }
        /// <summary>
        /// NewsFlash
        /// </summary>
        public bool NewsFlash
        {
            get { return m_NewsFlash; }
            set { m_NewsFlash = value; }
        }
        /// <summary>
        /// Subtitle
        /// </summary>
        public bool Subtitle
        {
            get { return m_Subtitle; }
            set { m_Subtitle = value; }
        }
        private bool m_HdSupport = false;
        /// <summary>
        /// HdSupport
        /// </summary>
        public bool HdSupport
        {
            get { return m_HdSupport; }
            set { m_HdSupport = value; }
        }
        private bool m_Inhibit = false;
        /// <summary>
        /// Inhibit
        /// </summary>
        public bool Inhibit
        {
            get { return m_Inhibit; }
            set { m_Inhibit = value; }
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
        /// Initializes a new instance of the VortexAttributes class.
        /// </summary>
        public PageAttributes(EP1File ep1File)
        {
            this.Parse(ep1File);
        }
        #endregion

        #region Privat Methods
        //**************************************************
        // Privat Methods
        //**************************************************

        /// <summary>
        /// Parse EP1-File and set properties
        /// </summary>
        /// <param name="ep1File"></param>
        private void Parse(EP1File ep1File)
        {
            // set magazine, set & page number
            string pageLine = this.GetString(ep1File, 1, 24, 11);
            this.Magazine = int.Parse(pageLine.Trim().Split(' ')[0]);
            pageLine = pageLine.Split(' ')[1];
            this.Set = int.Parse(pageLine.Trim().Split('.')[0]);
            this.Page = int.Parse(pageLine.Trim().Split('.')[1]);

            // set page properties
            this.Title = this.GetString(ep1File, 2, 24, 17);
            this.Autosequence = this.GetString(ep1File, 4, 16, 11);
            this.Timestamp = this.GetString(ep1File, 5, 16, 11);
            this.SelectCount = this.GetInteger(ep1File, 7, 16, 11);
            this.Frequency = this.GetInteger(ep1File, 8, 16, 11);
            this.Alias = this.GetString(ep1File, 10, 16, 11);
            this.Subcode = this.GetString(ep1File, 11, 16, 11);
            this.Autosource = this.GetString(ep1File, 12, 16, 11);
            this.Autobatch = this.GetString(ep1File, 13, 16, 11);
            this.CopyProtect = this.GetString(ep1File, 14, 16, 11);
            this.IPProtect = this.GetString(ep1File, 15, 11, 29);
            this.Protect = this.GetString(ep1File, 16, 11, 29);
            this.Advert = this.GetString(ep1File, 17, 11, 29);
            this.Access = this.GetString(ep1File, 19, 10, 31);
            this.Owner = this.GetString(ep1File, 20, 17, 23);
            this.LastUpdate = Convert.ToDateTime(this.GetString(ep1File, 22, 29, 8) + " " + this.GetString(ep1File, 22, 17, 8));
            this.Language = this.GetString(ep1File, 21, 17, 23);
            this.LastCommand = this.GetString(ep1File, 23, 17, 23);

            // set page flags
            this.Included = this.GetBool(ep1File, 5, 36, 1);
            this.Archive = this.GetBool(ep1File, 6, 36, 1);
            this.Clear = this.GetBool(ep1File, 8, 36, 1);
            this.Update = this.GetBool(ep1File, 9, 36, 1);
            this.NewsFlash = this.GetBool(ep1File, 10, 36, 1);
            this.Subtitle = this.GetBool(ep1File, 11, 36, 1);
            this.HdSupport = this.GetBool(ep1File, 12, 36, 1);
            this.Inhibit = this.GetBool(ep1File, 13, 36, 1);
        }

        #endregion

        #endregion
    }

}
/* 
 * -------------------------------------------------------------------
 * History:
 * $Log: /CASPer/03_SW/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/VortexAttributesValues.cs $
 * 
 * 3     17.07.09 14:43 san
 * 
 * 2     16.07.09 16:27 san
 * 
 * 1     16.07.09 11:42 san
 * -------------------------------------------------------------------
 */