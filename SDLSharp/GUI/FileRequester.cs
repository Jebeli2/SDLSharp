using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{
    public class FileRequester : ASLRequester
    {

        private readonly Gadget okButton;
        private readonly Gadget cancelButton;
        private readonly Gadget volumesButton;
        private readonly Gadget parentButton;
        private readonly Gadget fileNameGadget;
        private readonly Gadget dirNameGadget;
        private readonly Gadget listViewGadget;
        private readonly ListViewInfo listViewInfo;

        private const int WIDTH = 500;
        private const int HEIGHT = 500;
        private const int BUTTONWIDTH = WIDTH / 4 - 2;
        private const int BUTTONLEFT = 1;
        private DirectoryInfo? directoryInfo;

        public FileRequester()
        {


            okButton = GadTools.CreateGadget(GadgetKind.Button, BUTTONLEFT, -30, BUTTONWIDTH, 30, "Open", clickAction: OkSelected);
            volumesButton = GadTools.CreateGadget(GadgetKind.Button, BUTTONWIDTH + BUTTONLEFT, -30, BUTTONWIDTH, 30, "Volumes", clickAction: GoToVolumes);
            parentButton = GadTools.CreateGadget(GadgetKind.Button, BUTTONWIDTH * 2 + BUTTONLEFT, -30, BUTTONWIDTH, 30, "Parent", clickAction: GoToParent);
            cancelButton = GadTools.CreateGadget(GadgetKind.Button, BUTTONWIDTH * 3 + BUTTONLEFT, -30, -BUTTONWIDTH * 3, 30, "Cancel", clickAction: CancelSelected);
            var fileLabel = GadTools.CreateGadget(GadgetKind.Text, BUTTONLEFT, -60, BUTTONWIDTH, 30, "File:");
            fileNameGadget = GadTools.CreateGadget(GadgetKind.String, BUTTONWIDTH, -60, -(BUTTONWIDTH + BUTTONLEFT), 30);
            var dirLabel = GadTools.CreateGadget(GadgetKind.Text, BUTTONLEFT, -90, BUTTONWIDTH, 30, "Drawer:");
            dirNameGadget = GadTools.CreateGadget(GadgetKind.String, BUTTONWIDTH, -90, -(BUTTONWIDTH + BUTTONLEFT), 30);
            listViewGadget = GadTools.CreateGadget(GadgetKind.ListView, BUTTONLEFT, 1, -BUTTONLEFT * 2, -90);

            NewWindow = new NewWindow
            {
                LeftEdge = 10,
                TopEdge = 10,
                Width = WIDTH + 8,
                Height = HEIGHT + 8,
                Title = "Open File",
                Gadgets = new Gadget[] { okButton, volumesButton, parentButton, cancelButton, fileLabel, fileNameGadget, dirLabel, dirNameGadget, listViewGadget },
                MinWidth = WIDTH + 8,
                MinHeight = HEIGHT + 8,
                Activate = true,
                SuperBitmap = true,
                Sizing = true,
                Dragging = true,
                Closing = true,
                CloseAction = CloseSelected
            };
            listViewInfo = listViewGadget.GadInfo?.ListViewInfo!;
            listViewInfo.AddColumn("Name", -1);
            listViewInfo.AddColumn("Size", 100);
            listViewInfo.AddColumn("Date", 150);
            listViewInfo.AddColumn("Comment", 64);
            listViewInfo.SelectedIndexChanged = SelectedIndexChanged;
            listViewInfo.IndexDoubleClicked = IndexDoubleClicked;
        }

        private void SelectedIndexChanged(int index)
        {
            var row = listViewInfo.GetRow(index);
            FileSystemInfo? info = row?.Tag as FileSystemInfo;
            if (info != null)
            {
                GadTools.SetAttrs(dirNameGadget, buffer: Path.GetDirectoryName(info.FullName));
                GadTools.SetAttrs(fileNameGadget, buffer: Path.GetFileName(info.FullName));
            }
        }

        private void IndexDoubleClicked(int index)
        {
            var row = listViewInfo.GetRow(index);
            FileSystemInfo? info = row?.Tag as FileSystemInfo;
            if (info != null)
            {
                if (info is DirectoryInfo dirInfo)
                {
                    FillListView(dirInfo);
                }
            }

        }

        internal override void Init(string? dir = null)
        {
            if (dir != null)
            {
                if (Directory.Exists(dir))
                {

                    DirectoryInfo dirInfo = new DirectoryInfo(dir);
                    FillListView(dirInfo);
                }
            }
        }
        internal void FillListView(DirectoryInfo dirInfo)
        {
            directoryInfo = dirInfo;
            FillListView(directoryInfo.EnumerateFileSystemInfos());
            GadTools.SetAttrs(dirNameGadget, buffer: directoryInfo.FullName);
            GadTools.SetAttrs(fileNameGadget, buffer: "");
        }
        internal void FillListView(IEnumerable<FileSystemInfo> infos)
        {
            //listViewInfo.InitColumnWidths();
            //listViewInfo.InitColumns();
            listViewInfo.ClearRows();
            foreach (FileSystemInfo info in infos.OrderByDescending(x => (x.Attributes & FileAttributes.Directory)).ThenBy(x => x.Name))
            {
                if (info is DirectoryInfo dirInfo)
                {
                    string name = info.Name;
                    string sizeS = "Drawer";
                    string dateS = info.LastWriteTime.ToString("yyyy:MM:dd HH:mm:ss");
                    var row = listViewInfo.AddRow(name, sizeS, dateS);
                    row.Tag = dirInfo;
                }
                else if (info is FileInfo fileInfo)
                {
                    string name = info.Name;
                    string sizeS = fileInfo.Length.ToString();
                    string dateS = info.LastWriteTime.ToString("yyyy:MM:dd HH:mm:ss");
                    var row = listViewInfo.AddRow(name, sizeS, dateS);
                    row.Tag = fileInfo;
                }
            }
        }

        private void OkSelected()
        {
            CloseWindow();
        }

        private void GoToVolumes()
        {

        }

        private void GoToParent()
        {
            if (directoryInfo != null)
            {
                var parent = directoryInfo.Parent;
                if (parent != null)
                {
                    FillListView(parent);
                }
            }
        }

        private void CancelSelected()
        {
            CloseWindow();
        }

        private void CloseSelected()
        {
            CloseWindow();
        }

        private void CloseWindow()
        {
            Intuition.CloseWindow(ref Window);
        }
    }
}
