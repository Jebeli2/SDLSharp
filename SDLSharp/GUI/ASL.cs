using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{
    public static class ASL
    {
        public static void ActivateAslRequest(ASLRequester? requester)
        {
            if (requester != null && requester.Window != null)
            {
                Intuition.ActivateWindow(requester.Window);
            }
        }

        public static void AbortAslRequest(ASLRequester? requester)
        {
            if (requester != null && requester.Window != null)
            {
                Intuition.CloseWindow(requester.Window);
                requester.Window = null;
            }
        }
        public static bool AslRequest(ASLRequester? requester, string? dir = null)
        {
            if (requester != null && requester.NewWindow != null)
            {
                requester.Window = Intuition.OpenWindow(requester.NewWindow);
                requester.Init(dir);
                return true;
            }
            return false;
        }

        public static ASLRequester? AllocAslRequest(ASLRequestType requestType)
        {
            switch (requestType)
            {
                case ASLRequestType.FileRequest: return AllocFileRequester();
            }
            return null;
        }

        public static void FreeAslRequest(ASLRequester? requester)
        {
            if (requester != null)
            {
                if (requester.Window != null)
                {
                    AbortAslRequest(requester);
                }
            }
        }

        private static FileRequester AllocFileRequester()
        {
            FileRequester req = new FileRequester();
            return req;
        }
    }
}
