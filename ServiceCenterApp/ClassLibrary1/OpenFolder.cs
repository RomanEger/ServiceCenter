﻿using System.Windows.Forms;

namespace ClassLibrary1;

public class OpenFolder
{
    public static FolderBrowserDialog CreateFolderDialog() => new FolderBrowserDialog();
}