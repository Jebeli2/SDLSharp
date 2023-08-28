namespace SDLSharp.GUI
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;

    public static class GadTools
    {
        private const int CHECKBOX_WIDTH = 28;
        private const int CHECKBOX_HEIGHT = 22;
        private const int INTERWIDTH = 8;

        public static Gadget CreateGadget(GadgetKind kind,
            int leftEdge = 0,
            int topEdge = 0,
            int width = 100,
            int height = 50,
            string? text = null,
            Icons icon = Icons.NONE,
            Color? bgColor = null,
            string? buffer = null,
            int intValue = 0,
            bool disabled = false,
            bool selected = false,
            bool toggleSelect = false,
            bool _cheked = false,
            bool endGadget = false,
            Action? clickAction = null,
            Action<int>? valueChangedAction = null,
            Action<bool>? checkedStateChangedAction = null,
            int min = 0,
            int max = 15,
            int level = 0,
            int top = 0,
            int total = 0,
            int visible = 2,
            PropFreedom freedom = PropFreedom.Horizontal,
            bool scaled = false
            )

        {
            switch (kind)
            {
                case GadgetKind.Button: return CreateButton(leftEdge, topEdge, width, height, text, icon, bgColor, disabled, selected, toggleSelect, clickAction);
                case GadgetKind.Checkbox: return CreateCheckbox(leftEdge, topEdge, width, height, text, _cheked, checkedStateChangedAction, scaled);
                case GadgetKind.Text: return CreateText(leftEdge, topEdge, width, height, text);
                case GadgetKind.Number: return CreateNumber(leftEdge, topEdge, width, height, intValue, text ?? "{0}");
                case GadgetKind.Slider: return CreateSlider(leftEdge, topEdge, width, height, min, max, level, freedom, valueChangedAction);
                case GadgetKind.Scroller: return CreateScroller(leftEdge, topEdge, width, height, top, total, visible, freedom, valueChangedAction);
                case GadgetKind.String: return CreateString(leftEdge, topEdge, width, height, buffer);
                case GadgetKind.Integer: return CreateInteger(leftEdge, topEdge, width, height, intValue);
            }
            throw new NotSupportedException($"GadgetKind {kind} not supported");
        }

        public static void SetAttrs(Gadget gadget, int? intValue = null)
        {
            switch (GetGadgetKind(gadget))
            {
                case GadgetKind.Number: SetNumberAttrs(gadget, intValue); break;
            }
        }
        private static void SetNumberAttrs(Gadget gadget, int? intValue)
        {
            if (IsValid(gadget, GadgetKind.Number, out GadToolsInfo? info))
            {
                if (intValue != null)
                {
                    string format = info.Format ?? "{0}";
                    gadget.Text = string.Format(format, intValue.Value);
                }
            }
        }

        private static Gadget CreateButton(int leftEdge, int topEdge, int width, int height,
            string? text, Icons icon, Color? bgColor,
            bool disabled,
            bool selected,
            bool toggleSelect,
            Action? clickAction
        )
        {
            Gadget gadget = new Gadget(GadgetKind.Button)
            {
                LeftEdge = leftEdge,
                TopEdge = topEdge,
                Width = width,
                Height = height,
                Text = text,
                Icon = icon,
                BackgroundColor = bgColor ?? Color.Empty,
                Disbled = disabled,
                Selected = selected,
                ToggleSelect = toggleSelect
            };
            if (clickAction != null)
            {
                gadget.GadgetUp += (s, e) => { clickAction(); };
            }
            return gadget;
        }

        private static Gadget CreateCheckbox(int leftEdge, int topEdge, int width, int height,
            string? text, bool _checked, Action<bool>? checkedStateChangedAction, bool scaled)
        {
            Gadget gadget = new Gadget(GadgetKind.Checkbox)
            {
                LeftEdge = leftEdge,
                TopEdge = topEdge,
                Width = width,
                Height = scaled ? height : CHECKBOX_HEIGHT,
                Text = text,
                GadgetType = GadgetType.CustomGadget
            };
            if (gadget.GadInfo != null)
            {
                gadget.GadInfo.CheckboxChecked = _checked;
                gadget.GadInfo.Scaled = scaled;
                gadget.GadInfo.CheckedStateChangedAction = checkedStateChangedAction;
            }
            gadget.CustomRenderAction = RenderCheckbox;
            gadget.GadgetUp += CheckboxGadgetUp;
            return gadget;
        }


        private static Gadget CreateText(int leftEdge, int topEdge, int width, int height,
            string? text)
        {
            Gadget gadget = new Gadget(GadgetKind.Text)
            {
                LeftEdge = leftEdge,
                TopEdge = topEdge,
                Width = width,
                Height = height,
                Text = text,
                TabCycle = false,
                NoHighlight = true,
                TransparentBackground = true
            };
            return gadget;
        }
        private static Gadget CreateNumber(int leftEdge, int topEdge, int width, int height, int intValue, string format)
        {
            Gadget gadget = new Gadget(GadgetKind.Number)
            {
                LeftEdge = leftEdge,
                TopEdge = topEdge,
                Width = width,
                Height = height,
                Text = string.Format(format, intValue),
                TabCycle = false,
                NoHighlight = true,
                TransparentBackground = true
            };
            if (gadget.GadInfo != null)
            {
                gadget.GadInfo.Format = format;
            }
            return gadget;
        }

        private static Gadget CreateString(int leftEdge, int topEdge, int width, int height, string? buffer)
        {
            Gadget gadget = new Gadget(GadgetKind.String)
            {
                LeftEdge = leftEdge,
                TopEdge = topEdge,
                Width = width,
                Height = height,
                GadgetType = GadgetType.StrGadget,
            };
            if (gadget.StringInfo != null && buffer != null)
            {
                gadget.StringInfo.Buffer = buffer;
            }
            return gadget;
        }
        private static Gadget CreateInteger(int leftEdge, int topEdge, int width, int height, int intValue)
        {
            Gadget gadget = new Gadget(GadgetKind.String)
            {
                LeftEdge = leftEdge,
                TopEdge = topEdge,
                Width = width,
                Height = height,
                LongInt = true,
                GadgetType = GadgetType.StrGadget,
            };
            if (gadget.StringInfo != null)
            {
                gadget.StringInfo.Buffer = intValue.ToString();
            }
            return gadget;
        }

        private static Gadget CreateSlider(int leftEdge, int topEdge, int width, int height, int min, int max, int level, PropFreedom freedom,
            Action<int>? valueChangedAction)
        {
            Gadget gadget = new Gadget(GadgetKind.Slider)
            {
                LeftEdge = leftEdge,
                TopEdge = topEdge,
                Width = width,
                Height = height,
                GadgetType = GadgetType.PropGadget,
            };
            PropFlags flags = 0;
            if ((freedom & PropFreedom.Horizontal) != 0) { flags |= PropFlags.FreeHoriz; }
            if ((freedom & PropFreedom.Vertical) != 0) { flags |= PropFlags.FreeVert; }
            if (level > max) { level = max; }
            if (level < min) { level = min; }
            FindSliderValues(max + 1 - min, level - min, out int body, out int pot);
            Intuition.ModifyProp(gadget, flags, pot, pot, body, body);
            if (gadget.GadInfo != null)
            {
                gadget.GadInfo.SliderMin = min;
                gadget.GadInfo.SliderMax = max;
                gadget.GadInfo.SliderLevel = level;
                gadget.GadInfo.ValueChangedAction = valueChangedAction;
            }
            gadget.GadgetDown += SliderGadgetDown;
            gadget.GadgetUp += SliderGadgetUp;
            return gadget;
        }


        private static Gadget CreateScroller(int leftEdge, int topEdge, int width, int height, int top, int total, int visible, PropFreedom freedom,
            Action<int>? valueChangedAction)
        {
            Gadget gadget = new Gadget(GadgetKind.Scroller)
            {
                LeftEdge = leftEdge,
                TopEdge = topEdge,
                Width = width,
                Height = height,
                GadgetType = GadgetType.PropGadget,
            };
            PropFlags flags = 0;
            if ((freedom & PropFreedom.Horizontal) != 0) { flags |= PropFlags.FreeHoriz; }
            if ((freedom & PropFreedom.Vertical) != 0) { flags |= PropFlags.FreeVert; }
            FindScrollerValues(total, visible, top, 0, out int body, out int pot);
            Intuition.ModifyProp(gadget, flags, pot, pot, body, body);
            if (gadget.GadInfo != null)
            {
                gadget.GadInfo.ScrollerTop = top;
                gadget.GadInfo.ScrollerTotal = total;
                gadget.GadInfo.ScrollerVisible = visible;
                gadget.GadInfo.ValueChangedAction = valueChangedAction;
            }
            gadget.GadgetDown += ScrollerGadgetDown;
            gadget.GadgetUp += ScrollerGadgetUp;
            return gadget;

        }

        private static void CheckboxGadgetUp(object? sender, EventArgs e)
        {
            CheckboxCheckChanged(sender as Gadget);
        }

        private static void CheckboxCheckChanged(Gadget? gadget)
        {
            if (gadget != null && gadget.GadInfo != null)
            {
                bool check = !gadget.GadInfo.CheckboxChecked;
                gadget.GadInfo.CheckboxChecked = check;
                gadget.GadInfo.CheckedStateChangedAction?.Invoke(check);
            }
        }

        private static void SliderGadgetUp(object? sender, EventArgs e)
        {
            SliderPotChanged(sender as Gadget);
        }

        private static void SliderGadgetDown(object? sender, EventArgs e)
        {
            SliderPotChanged(sender as Gadget);
        }
        private static void SliderPotChanged(Gadget? gadget)
        {
            if (gadget != null && GetPot(gadget, out int pot))
            {
                if (gadget.GadInfo != null)
                {
                    int level = FindSliderLevel(gadget.GadInfo.SliderMax + 1 - gadget.GadInfo.SliderMin, pot) + gadget.GadInfo.SliderMin;
                    if (level != gadget.GadInfo.SliderLevel)
                    {
                        gadget.GadInfo.SliderLevel = level;
                        gadget.GadInfo.ValueChangedAction?.Invoke(level);
                    }
                }
            }
        }

        private static void FindSliderValues(int numLevels, int level, out int body, out int pot)
        {
            if (numLevels > 0)
            {
                body = PropInfo.MAXBODY / numLevels;
            }
            else
            {
                body = PropInfo.MAXBODY;
            }
            if (numLevels > 1)
            {
                pot = (PropInfo.MAXPOT * level) / (numLevels - 1);
            }
            else
            {
                pot = 0;
            }
        }

        private static int FindSliderLevel(int numLevels, int pot)
        {
            if (numLevels > 1)
            {
                return (pot * (numLevels - 1) + PropInfo.MAXPOT / 2) / PropInfo.MAXPOT;
            }
            else
            {
                return 0;
            }
        }


        private static void ScrollerGadgetUp(object? sender, EventArgs e)
        {
            ScrollerPotChanged(sender as Gadget);
        }

        private static void ScrollerGadgetDown(object? sender, EventArgs e)
        {
            ScrollerPotChanged(sender as Gadget);
        }

        private static void ScrollerPotChanged(Gadget? gadget)
        {
            if (gadget != null && GetPot(gadget, out int pot))
            {
                if (gadget.GadInfo != null)
                {
                    int top = FindScrollerTop(gadget.GadInfo.ScrollerTotal, gadget.GadInfo.ScrollerVisible, pot);
                    if (top != gadget.GadInfo.ScrollerTop)
                    {
                        gadget.GadInfo.ScrollerTop = top;
                        gadget.GadInfo.ValueChangedAction?.Invoke(top);
                    }
                }
            }

        }
        private static void FindScrollerValues(int total, int displayable, int top, int overlap, out int body, out int pot)
        {
            int hidden = Math.Max(total - displayable, 0);
            if (top > hidden) { top = hidden; }
            body = (hidden > 0) ? ((displayable - overlap) * PropInfo.MAXBODY) / (total - overlap) : PropInfo.MAXBODY;
            pot = (hidden > 0) ? (top * PropInfo.MAXBODY) / hidden : 0;
        }
        private static int FindScrollerTop(int total, int displayable, int pot)
        {
            int hidden = Math.Max(total - displayable, 0);
            return ((hidden * pot) + (PropInfo.MAXPOT / 2)) / PropInfo.MAXPOT;
        }

        private static GadgetKind GetGadgetKind(Gadget? gadget)
        {
            return gadget?.GadInfo?.Kind ?? GadgetKind.None;
        }
        private static bool GetPot(Gadget gadget, out int pot)
        {
            pot = 0;
            if (gadget.IsPropGadget && gadget.PropInfo != null)
            {
                if (gadget.PropInfo.FreeVert)
                {
                    pot = gadget.PropInfo.VertPot;
                    return true;
                }
                else if (gadget.PropInfo.FreeHoriz)
                {
                    pot = gadget.PropInfo.HorizPot;
                    return true;
                }
            }
            return false;
        }

        private static bool IsValid(Gadget? gadget, GadgetKind expectedKind, [NotNullWhen(true)] out GadToolsInfo? info)
        {
            info = null;
            if (gadget != null)
            {
                info = gadget.GadInfo;
                if (info != null)
                {
                    return info.Kind == expectedKind;
                }
            }
            return false;
        }

        private static void RenderCheckbox(IGuiRenderer gui, SDLRenderer gfx, Gadget gadget, int offsetX, int offsetY)
        {
            if (IsValid(gadget, GadgetKind.Checkbox, out GadToolsInfo? info))
            {
                Rectangle bounds = gadget.GetBounds();
                Rectangle inner = gadget.GetInnerBounds();
                bounds.Offset(offsetX, offsetY);
                inner.Offset(offsetX, offsetY);
                bool scaled = info.Scaled;
                Rectangle box = bounds;
                box.Width = CHECKBOX_WIDTH;
                if (!scaled)
                {
                    box.Height = CHECKBOX_HEIGHT;
                    box.Y += (bounds.Height - box.Height) / 2;
                }
                gui.RenderGadgetBorder(gfx, box, gadget.Active, gadget.MouseHover, gadget.Selected);
                if (info.CheckboxChecked)
                {
                    gfx.DrawIcon(Icons.CHECK, box.X, box.Y, box.Width, box.Height, Color.White);
                }
                Rectangle textBox = inner;
                textBox.X += (INTERWIDTH + CHECKBOX_WIDTH);
                textBox.Width -= (INTERWIDTH + CHECKBOX_WIDTH);
                gfx.DrawText(null, gadget.Text, textBox.X, textBox.Y, textBox.Width, textBox.Height, Color.White, HorizontalAlignment.Left);
            }
        }

    }
}
