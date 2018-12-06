using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetExampleApp.Database
{
    public class FileType
    {
        public static String ATTR_id = "id";
        public static String ATTR_extension = "extension";
        public static String ATTR_mime = "mime";

        public static int LEN_ATTR_extension = 5;
        public static int LEN_ATTR_mime = 20;

        private int mId;
        private String mExtension;
        private String mMime;

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

        public String Extension
        {
            get
            {
                return mExtension;
            }
            set
            {
                mExtension = value;
            }
        }

        public String Mime
        {
            get
            {
                return mMime;
            }
            set
            {
                mMime = value;
            }
        }
    }
}