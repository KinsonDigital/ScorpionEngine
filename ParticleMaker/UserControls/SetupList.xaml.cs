using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for SetupList.xaml
    /// </summary>
    public partial class SetupList : UserControl
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SetupList"/>.
        /// </summary>
        public SetupList()
        {
            InitializeComponent();
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="ProjectPath"/> property.
        /// </summary>
        public static readonly DependencyProperty ProjectPathProperty =
            DependencyProperty.Register(nameof(ProjectPath), typeof(string), typeof(SetupList), new PropertyMetadata("", ProjectPathChanged));

        /// <summary>
        /// Registers the <see cref="Setups"/> property.
        /// </summary>
        protected static readonly DependencyProperty SetupProperty =
            DependencyProperty.Register(nameof(Setups), typeof(SetupPathItem[]), typeof(SetupList), new PropertyMetadata(new SetupPathItem[0]));
        #endregion


        /// <summary>
        /// Gets or sets the path to the project that contains the setups.
        /// </summary>
        public string ProjectPath
        {
            get { return (string)GetValue(ProjectPathProperty); }
            set { SetValue(ProjectPathProperty, value); }
        }


        /// <summary>
        /// Gets or sets the list of setup paths.
        /// </summary>
        protected SetupPathItem[] Setups
        {
            get { return (SetupPathItem[])GetValue(SetupProperty); }
            set { SetValue(SetupProperty, value); }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Updates the list of setups.
        /// </summary>
        private static void ProjectPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var result = new List<SetupPathItem>();

            var projPath = (string)e.NewValue;

            if (string.IsNullOrEmpty(projPath))
                return;

            var ctrl = (SetupList)d;

            if (ctrl == null)
                return;

            if (DesignerProperties.GetIsInDesignMode(ctrl))
            {
                for (int i = 1; i <= 4; i++)
                {
                    result.Add(new SetupPathItem() { FilePath = $"setup-{i}.json" });
                }
            }
            else
            {
                if (ProjectPathExists(projPath))
                {
                    var setups = Directory.GetFiles(projPath);

                    foreach (var setup in setups)
                    {
                        result.Add(new SetupPathItem() { FilePath = setup });
                    }
                }
            }


            ctrl.Setups = result.ToArray();
        }


        /// <summary>
        /// Returns true if the given path exists.
        /// </summary>
        /// <param name="path">The path to check for.</param>
        /// <returns></returns>
        private static bool ProjectPathExists(string path)
        {
            return Directory.Exists(path);
        }
        #endregion
    }
}
