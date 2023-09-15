namespace SDLSharp.Maps;

using System;
using System.Collections.Generic;

public class TooltipData : IEquatable<TooltipData>
{
    private readonly List<string> lines = new();
    private SDLFont? font;

    public TooltipData()
    {

    }
    public TooltipData(string line)
    {
        SetText(line);
    }
    public void SetText(string line)
    {
        Clear();
        AddText(line);
    }
    public void AddText(string line)
    {
        lines.Add(line);
    }
    public SDLFont? Font
    {
        get { return font; }
        set { font = value; }
    }

    public IList<string> Lines => lines;
    public bool IsEmpty => lines.Count == 0;
    public void Clear()
    {
        lines.Clear();
    }

    public static bool Equals(TooltipData? t1, TooltipData? t2)
    {
        if (t1 == null)
        {
            if (t2 == null) { return true; } else { return false; }
        }
        else if (t2 == null)
        {
            if (t1 == null) { return true; } else { return false; }
        }
        else
        {
            if (t1.IsEmpty && t2.IsEmpty) return true;
            if (t1.IsEmpty && !t2.IsEmpty) return false;
            if (!t1.IsEmpty && t2.IsEmpty) return false;
            return t1.Equals(t2);
        }
        //if (t1 == null && t2 == null) return true;
        //if (t1 == null && t2 != null) return false;
        //if (t1 != null && t2 == null) return false;
        //if (t1.IsEmpty && t2.IsEmpty) return true;
        //if (t1.IsEmpty && !t2.IsEmpty) return false;
        //if (!t1.IsEmpty && t2.IsEmpty) return false;
        //return t1.Equals(t2);
    }

    public bool Equals(TooltipData? other)
    {
        if (other == null) return false;
        string txt = string.Join(',', lines);
        string otxt = string.Join(',', other.lines);
        return string.Equals(txt, otxt);
    }

    public override bool Equals(object? obj)
    {
        if (obj is TooltipData td) { return Equals(td); }
        return false;
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

    public override string ToString()
    {
        return string.Join(',', lines);
    }
}
