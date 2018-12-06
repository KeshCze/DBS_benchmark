using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetExampleApp.Database
{
    public class PaperVersion
    {
        public static String ATTR_id = "id";
        public static String ATTR_uploadDate = "uploadDate";
        public static String ATTR_idFileType = "idFileType";

        private int mId;
        private DateTime mUploadDate;
        private int mIdFileType = 0;

        private FileType mFileType;

        public int Id
        {
            get
            {
                return mId;
            }
            set
            {
                mId = value;
            }
        }

        public DateTime UploadDate
        {
            get
            {
                return mUploadDate;
            }
            set
            {
                mUploadDate = value;
            }
        }

        public int IdFileType
        {
            get
            {
                return mIdFileType;
            }
            set
            {
                mIdFileType = value;
            }
        }

        public FileType FileType
        {
            get
            {
                return mFileType;
            }
            set
            {
                mFileType = value;
            }
        }
    }
}

