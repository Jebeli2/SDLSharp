using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{
    internal class ListViewInfo
    {
        private readonly Gadget gadget;
        private bool evenColumns = false;
        private bool showHeader = true;
        private int headerHeight = 30;
        private int rowHeight = 28;
        private readonly List<ListViewColumn> columns = new();
        private readonly List<ListViewRow> rows = new();
        private bool validated;
        private ListViewCell? hoverCell;
        private ListViewCell? selectedCell;
        private ListViewColumn? hoverColumn;
        private ListViewColumn? selectedColumn;
        private TableSelectMode selectMode = TableSelectMode.Rows;
        private Gadget scrollBar;
        public ListViewInfo(Gadget gadget)
        {
            this.gadget = gadget;
            scrollBar = GadTools.CreateGadget(GadgetKind.Scroller, 0, 0, 20, 20);
        }

        public Action<int>? SelectedIndexChanged;
        public Action<int>? IndexDoubleClicked;

        public void Invalidate()
        {
            validated = false;
        }

        public Gadget Gadget => gadget;

        public Rectangle HeaderRect
        {
            get
            {
                if (showHeader)
                {
                    var rect = gadget.GetBounds();
                    rect.Inflate(-1, -1);
                    rect.Height = headerHeight;
                    return rect;
                }
                return Rectangle.Empty;
            }
        }

        public Rectangle ViewRect
        {
            get
            {
                var rect = gadget.GetBounds();
                rect.Inflate(-1, -1);
                if (showHeader)
                {
                    rect.Height -= headerHeight;
                    rect.Y += headerHeight;
                }
                return rect;
            }
        }

        public int Width
        {
            get { return gadget.GetBounds().Width; }
        }
        public void ClearRows()
        {
            selectedCell = null;
            selectedColumn = null;
            rows.Clear();
        }

        public ListViewColumn AddColumn(string label, int width)
        {
            ListViewColumn col = new ListViewColumn(this)
            {
                Label = label,
                Width = width,
                PixelWidth = width,
                HTextAlign = HorizontalAlignment.Left,
                VTextAlign = VerticalAlignment.Center
            };
            columns.Add(col);
            return col;
        }

        public void InitColumns()
        {
            int x = 0;
            int index = 0;
            foreach (var col in columns)
            {
                col.X = x;
                col.Index = index;
                x += col.PixelWidth;
                index++;
            }
        }

        public void InitColumnWidths()
        {
            if (evenColumns)
            {

            }
            else
            {
                int numColumns = Math.Max(1, columns.Count);
                int w = Width - 2;
                int numAutoColumns = numColumns;
                if (numColumns > 0)
                {
                    for (int i = 0; i < numColumns; i++)
                    {
                        if (i < columns.Count)
                        {
                            int cw = columns[i].Width;
                            if (cw >= 0)
                            {
                                columns[i].PixelWidth = cw;
                                w -= cw;
                                numAutoColumns--;
                            }
                            else
                            {
                                columns[i].PixelWidth = -1;
                            }
                        }
                    }
                    if (numAutoColumns > 0)
                    {
                        int acw = w / numAutoColumns;
                        for (int i = 0; i < numColumns; i++)
                        {
                            if (i < columns.Count)
                            {
                                if (columns[i].PixelWidth < 0)
                                {
                                    columns[i].PixelWidth = acw;
                                }
                            }
                        }
                    }
                }
            }
        }

        public int ScrollValue
        {
            get { return 0; }
            set { }
        }

        public int VisibleAmount
        {
            get { return gadget.GetBounds().Height - headerHeight; }
            set { }
        }

        public int FirstVisibleRow
        {
            get
            {
                int v = ScrollValue / rowHeight;
                return v;
            }
        }
        public int FirstVisibleRowMod
        {
            get
            {
                if (FirstVisibleRow > 0)
                {
                    int f = FirstVisibleRow * rowHeight;
                    int v = ScrollValue - f;
                    return v;
                }
                return 0;
            }
        }
        public int NumVisibleRows
        {
            get
            {
                int v = VisibleAmount / rowHeight + 2;
                return v;
            }
        }

        public int LastVisibleRow
        {
            get
            {
                return FirstVisibleRow + NumVisibleRows;
            }
        }

        public int LastColumnIndex
        {
            get { return columns.Count - 1; }
        }

        public int LastRowIndex
        {
            get { return rows.Count - 1; }
        }

        internal int VisibleHeaderHeight
        {
            get { return showHeader ? headerHeight : 0; }
        }

        public ListViewRow? GetRow(int index)
        {
            return rows.FirstOrDefault(x => x.Index == index);
        }

        public void SetCellHover(ListViewCell cell, bool hover)
        {
            switch (selectMode)
            {
                case TableSelectMode.Cells:
                    cell.Hover = hover;
                    break;
                case TableSelectMode.Rows:
                    foreach (var c in cell.Row.Cells)
                    {
                        c.Hover = hover;
                    }
                    break;
                case TableSelectMode.Cols:
                    int idx = cell.ColIndex;
                    foreach (var row in rows)
                    {
                        row.Cells[idx].Hover = hover;
                    }
                    break;
            }
        }
        public void SetCellSelected(ListViewCell cell, bool selected)
        {
            switch (selectMode)
            {
                case TableSelectMode.Cells:
                    cell.Selected = selected;
                    break;
                case TableSelectMode.Rows:
                    foreach (var c in cell.Row.Cells)
                    {
                        c.Selected = selected;
                    }
                    break;
                case TableSelectMode.Cols:
                    int idx = cell.ColIndex;
                    foreach (var row in rows)
                    {
                        row.Cells[idx].Selected = selected;
                    }
                    break;
            }
        }

        public void SetHoverColumn(ListViewColumn? column)
        {
            if (column != hoverColumn)
            {
                if (hoverColumn != null)
                {
                    hoverColumn.Hover = false;
                }
                hoverColumn = column;
                if (hoverColumn != null)
                {
                    hoverColumn.Hover = true;
                }
            }
        }

        public void SetSelectedColumn(ListViewColumn? column)
        {
            if (column != selectedColumn)
            {
                if (selectedColumn != null)
                {
                    selectedColumn.Selected = false;
                }
                selectedColumn = column;
                if (selectedColumn != null)
                {
                    selectedColumn.Selected = true;
                }

            }
        }

        public void SetHoverCell(ListViewCell? cell)
        {
            if (cell != hoverCell)
            {
                if (hoverCell != null)
                {
                    SetCellHover(hoverCell, false);
                }
                hoverCell = cell;
                if (hoverCell != null)
                {
                    SetCellHover(hoverCell, true);
                }
            }
        }

        public void SetSelectedCell(ListViewCell? cell)
        {
            if (cell != selectedCell)
            {
                if (selectedCell != null)
                {
                    SetCellSelected(selectedCell, false);
                }
                selectedCell = cell;
                if (selectedCell != null)
                {
                    SetCellSelected(selectedCell, true);
                }
                int index = selectedCell?.RowIndex ?? -1;
                SelectedIndexChanged?.Invoke(index);
            }
        }

        private int X2Col(int x)
        {
            if (x < 0) return -1;
            if (columns.Count == 0)
            {
                return 0;
            }
            foreach (var col in columns)
            {
                if (x < col.PixelWidth)
                {
                    return col.Index;
                }
                x -= col.PixelWidth;
            }
            return -1;
        }

        private int Y2Row(int y)
        {
            y -= 2;
            y -= VisibleHeaderHeight;
            if (y < 0) return -1;
            y += FirstVisibleRowMod;
            y /= rowHeight;
            y += FirstVisibleRow;
            return y;
        }
        private ListViewCell? XY2Cell(int x, int y)
        {
            x = X2Col(x);
            y = Y2Row(y);
            return GetCell(x, y);
        }

        private ListViewColumn? XY2Column(int x, int y)
        {
            if (y >= 0 && y < VisibleHeaderHeight)
            {
                x = X2Col(x);
                if (x >= 0 && x < columns.Count)
                {
                    return columns[x];
                }
            }
            return null;
        }

        public ListViewCell? GetCell(int x, int y)
        {
            if (y >= 0 && y < rows.Count)
            {
                if (columns.Count == 0)
                {
                    return rows[y].Cells[0];
                }
                if (x >= 0 && x < columns.Count)
                {
                    return rows[y].Cells[x];
                }
            }
            return null;
        }

        internal bool HandleMouseMove(Rectangle bounds, int x, int y)
        {
            ListViewCell? oldHoverCell = hoverCell;
            ListViewColumn? oldHoverColumn = hoverColumn;
            ListViewCell? cell = XY2Cell(x - bounds.X, y - bounds.Y);
            ListViewColumn? column = XY2Column(x - bounds.X, y - bounds.Y);
            SetHoverColumn(column);
            SetHoverCell(cell);
            return (cell != oldHoverCell) || (column != oldHoverColumn);
        }

        internal bool HandleMouseDown(Rectangle bounds, int x, int y, bool isTimerRepeat = false)
        {
            return false;
        }

        internal bool HandleMouseUp(Rectangle bounds, int x, int y)
        {
            ListViewCell? oldSelectedCell = selectedCell;
            ListViewColumn? oldSelectedColumn = selectedColumn;
            ListViewCell? cell = XY2Cell(x - bounds.X, y - bounds.Y);
            ListViewColumn? column = XY2Column(x - bounds.X, y - bounds.Y);
            if (cell != null && cell == oldSelectedCell) // double click...
            {
                IndexDoubleClicked?.Invoke(cell.RowIndex);
                return true;
            }
            SetSelectedColumn(column);
            SetSelectedCell(cell);
            return (cell != oldSelectedCell) || (column != oldSelectedColumn);
        }

        public ListViewRow AddRow(params string[] labels)
        {
            ListViewRow row = new ListViewRow(this)
            {

            };
            MakeCells(row);
            for (int i = 0; i < row.Cells.Count && i < labels.Length; i++)
            {
                row.Cells[i].Label = labels[i];
            }
            row.Index = rows.Count;
            rows.Add(row);
            return row;
        }

        private void MakeCells(ListViewRow row)
        {
            int num = Math.Max(1, columns.Count);
            int i = 0;
            while (row.Cells.Count < num)
            {
                ListViewCell cell = new ListViewCell(this, row);
                if (i < columns.Count) { cell.Column = columns[i]; }
                row.Cells.Add(cell);
                i++;
            }
            while (row.Cells.Count > num)
            {
                row.Cells.RemoveAt(row.Cells.Count - 1);
            }
        }

        private void Validate()
        {
            if (!validated)
            {
                InitColumnWidths();
                InitColumns();
                validated = true;
            }
        }

        public void Render(IGuiRenderer gui, SDLRenderer gfx, int offsetX, int offsetY)
        {
            var bounds = gadget.GetBounds();
            bounds.Offset(offsetX, offsetY);
            gui.RenderGadgetBorder(gfx, bounds, false, false, false);
            Validate();
            if (showHeader) { DrawListViewHeader(gui, gfx, offsetX, offsetY); }
            DrawListView(gui, gfx, offsetX, offsetY);
        }

        private void DrawListViewHeader(IGuiRenderer gui, SDLRenderer gfx, int offsetX, int offsetY)
        {
            var rect = HeaderRect;
            rect.Offset(offsetX, offsetY);
            int y = 0;
            int height = rect.Height;
            foreach (var col in columns)
            {
                Rectangle colBox = new Rectangle(rect.X + col.X, rect.Y + y, col.PixelWidth, height);
                gui.RenderGadgetBorder(gfx, colBox, false, col.Hover, col.Selected);
                colBox.Inflate(-1, -1);
                gfx.DrawText(gadget.Font, col.Label, colBox.X, colBox.Y, colBox.Width, colBox.Height, gui.TextColor, col.HTextAlign, col.VTextAlign);
            }
        }

        private void DrawListView(IGuiRenderer gui, SDLRenderer gfx, int offsetX, int offsetY)
        {
            var rect = ViewRect;
            rect.Offset(offsetX, offsetY);
            gui.RenderGadgetBorder(gfx, rect, false, false, false);
            rect.Inflate(-1, -1);
            gfx.PushClip(rect);
            int start = FirstVisibleRow;
            int end = LastVisibleRow;
            ListViewRow? row = null;
            for (int i = start; i < end && i < rows.Count; i++)
            {
                row = rows[i];
                DrawRow(row, gui, gfx, rect.X, rect.Y);
            }
            if (row != null)
            {
                if (row.Y + rowHeight < rect.Bottom)
                {
                    DrawEmptyRow(row, gui, gfx, rect.X, rect.Y);
                }
            }
            gfx.PopClip();
        }

        private void DrawRow(ListViewRow row, IGuiRenderer gui, SDLRenderer gfx, int offsetX, int offsetY)
        {
            foreach (var cell in row.Cells)
            {
                DrawCell(cell, gui, gfx, offsetX, offsetY);
            }
        }
        private void DrawEmptyRow(ListViewRow row, IGuiRenderer gui, SDLRenderer gfx, int offsetX, int offsetY)
        {
            foreach (var cell in row.Cells)
            {
                DrawEmptyCell(cell, gui, gfx, offsetX, offsetY);
            }
        }

        private void DrawCell(ListViewCell cell, IGuiRenderer gui, SDLRenderer gfx, int offsetX, int offsetY)
        {
            int x = cell.X + offsetX;
            int y = cell.Y + offsetY;
            int w = cell.Width;
            int h = cell.Height;
            Rectangle cellBox = new Rectangle(x, y, w, h);
            Color tg = gui.TextColor;
            Color bg = gui.ButtonGradientBotUnFocused;
            if (cell.Hover)
            {
                bg = gui.ButtonGradientBotHover;
            }
            if (cell.Selected)
            {
                bg = gui.ButtonGradientBotPushed;
                tg = gui.SelectedTextColor;
            }
            //gui.RenderGadgetBackground(gfx, cellBox, false, cell.Hover, cell.DrawSelected);
            gfx.FillRect(cellBox, bg);
            if (!cell.IsFirstInRow)
            {
                gfx.DrawLine(x - 1, y, x - 1, y + h, gui.BorderLight);
            }
            gfx.DrawLine(x + w - 2, y, x + w - 2, y + h, gui.BorderDark);
            if (!string.IsNullOrEmpty(cell.Label))
            {
                var textBox = cellBox;
                gfx.DrawText(gadget.Font, cell.Label, textBox.X, textBox.Y, textBox.Width, textBox.Height, tg, cell.Column?.HTextAlign ?? HorizontalAlignment.Left, cell.Column?.VTextAlign ?? VerticalAlignment.Center);
            }
        }
        private void DrawEmptyCell(ListViewCell cell, IGuiRenderer gui, SDLRenderer gfx, int offsetX, int offsetY)
        {
            int x = cell.X + offsetX;
            int y = cell.Y + cell.Height + offsetY;
            int w = cell.Width;
            int h = gadget.GetBounds().Height - cell.Y;
            Rectangle cellBox = new Rectangle(x, y, w, h);
            Color bg = gui.ButtonGradientBotUnFocused;
            gfx.FillRect(cellBox, bg);
            if (!cell.IsFirstInRow)
            {
                gfx.DrawLine(x - 1, y, x - 1, y + h, gui.BorderLight);
            }
            gfx.DrawLine(x + w - 2, y, x + w - 2, y + h, gui.BorderDark);

        }

        public class ListViewColumn
        {
            public ListViewColumn(ListViewInfo info)
            {
                Info = info;
            }
            public ListViewInfo Info { get; set; }
            public int Index { get; set; }
            public string Label { get; set; } = string.Empty;
            public int Width { get; set; }
            public int PixelWidth { get; set; }
            public HorizontalAlignment HTextAlign { get; set; }
            public VerticalAlignment VTextAlign { get; set; }
            public bool Hover { get; set; }
            public bool Selected { get; set; }
            public bool MouseSelected { get; set; }
            public bool DrawSelected { get { return Selected || MouseSelected; } }

            //public int Height
            //{
            //    get { return Table.VisibleHeaderHeight; }
            //}
            public int X { get; set; }
        }

        public class ListViewRow
        {
            private readonly List<ListViewCell> cells = new();
            public ListViewRow(ListViewInfo info)
            {
                Info = info;
            }

            public ListViewInfo Info { get; set; }
            public int Index { get; set; }
            public int Id { get; set; }
            public object? Tag { get; set; }
            public List<ListViewCell> Cells => cells;
            public int Y
            {
                get
                {
                    int y = 0;
                    int offset = Index - Info.FirstVisibleRow;
                    y += offset * Info.rowHeight;
                    y -= Info.FirstVisibleRowMod;
                    return y;
                }
            }
        }

        public class ListViewCell
        {
            public ListViewCell(ListViewInfo info, ListViewRow row)
            {
                Info = info;
                Row = row;
            }

            public ListViewInfo Info { get; set; }
            public ListViewRow Row { get; set; }
            public ListViewColumn? Column { get; set; }
            public string? Label { get; set; }
            public bool Hover { get; set; }
            public bool Selected { get; set; }
            public bool MouseSelected { get; set; }
            public bool DrawSelected { get { return Selected || MouseSelected; } }

            public int Width
            {
                get { return Column != null ? Column.PixelWidth : Info.Width; }
            }

            public int Height
            {
                get { return Info.rowHeight; }
            }

            public int X
            {
                get { return Column != null ? Column.X : 0; }
            }

            public int Y
            {
                get { return Row.Y; }
            }

            public int RowIndex
            {
                get { return Row.Index; }
            }

            public int ColIndex
            {
                get { return Column != null ? Column.Index : 0; }
            }

            public bool IsFirstInRow { get { return ColIndex == 0; } }
            public bool IsLastInRow { get { return ColIndex == Info.LastColumnIndex; } }
            public bool IsFirstInCol { get { return RowIndex == 0; } }
            public bool IsLastInCol { get { return RowIndex == Info.LastRowIndex; } }

        }
    }
}
