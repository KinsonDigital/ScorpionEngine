﻿using System.Windows.Input;

namespace ParticleMaker.Dialogs.DesignData
{
    public class InputDialogData
    {
        public string DialogTitle { get; set; }

        public string Message { get; set; }

        public string InputValue { get; set; }

        public ICommand OkCommand { get; set; }

        public ICommand CancelCommand { get; set; }
    }
}
