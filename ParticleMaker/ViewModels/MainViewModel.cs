using System;
using System.Windows.Forms;
using System.Windows.Threading;
using KDParticleEngine;
using CoreVector = KDScorpionCore.Vector;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using KDScorpionCore.Graphics;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Threading;
using ParticleMaker.Project;
using ParticleMaker.Dialogs;
using System.Windows;
using WPFMsgBox = System.Windows.MessageBox;
using ParticleMaker.UserControls;
using System.Linq;
using ParticleMaker.CustomEventArgs;

namespace ParticleMaker.ViewModels
{
    /// <summary>
    /// The main view model for the application.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Public Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region Private Fields
        private readonly char[] _illegalCharacters = new[] { '\\', '/', ':', '*', '?', '\"', '<', '>', '|', '.' };
        private readonly GraphicsEngine _graphicsEngine;
        private readonly ProjectManager _projectManager;
        private readonly ProjectSettingsManager _projectSettingsManager;
        private readonly SetupManager _setupManager;
        private readonly CancellationTokenSource _cancelTokenSrc;
        private readonly Task _startupTask;
        private RelayCommand _newProjectCommand;
        private RelayCommand _openProjectCommand;
        private ProjectSettings _projectSettings;
        private RelayCommand _setupItemSelectedCommand;
        private RelayCommand _addSetupCommand;
        private RelayCommand _renameProjectCommand;
        #endregion


        #region Constructor
        /// <summary>
        /// Creates a new instance of <see cref="MainViewModel"/>.
        /// </summary>
        /// <param name="renderSurface">The surface to render the graphics on.</param>
        /// <param name="uiDispatcher">The UI thread to start the graphics engine on.</param>
        [ExcludeFromCodeCoverage]
        public MainViewModel(GraphicsEngine graphicsEngine, ProjectManager projectManager,
            ProjectSettingsManager projectSettingsManager,
            SetupManager setupManager)
        {
            _graphicsEngine = graphicsEngine;
            _projectManager = projectManager;
            _projectSettingsManager = projectSettingsManager;
            _setupManager = setupManager;

            _cancelTokenSrc = new CancellationTokenSource();

            _startupTask = new Task(() =>
            {
                //If the cancellation has not been requested, keep processing.
                while (!_cancelTokenSrc.IsCancellationRequested)
                {
                    _cancelTokenSrc.Token.WaitHandle.WaitOne(250);

                    Action taskAction = new Action(() =>
                    {
                        if (_cancelTokenSrc.IsCancellationRequested == false && RenderSurface != null && RenderSurface.Handle != IntPtr.Zero)
                        {
                            _cancelTokenSrc.Cancel();

                            _graphicsEngine.RenderSurfaceHandle = RenderSurface.Handle;
                            _graphicsEngine.ParticleEngine.LivingParticlesCountChanged += _particleEngine_LivingParticlesCountChanged;
                            _graphicsEngine.ParticleEngine.SpawnLocation = new CoreVector(200, 200);

                            //NOTE: This line of code will not continue execution until the Monogame framework
                            //has stopped running
                            _graphicsEngine.Run();
                        }
                    });

                    if (UIDispatcher == null)
                    {
                        taskAction();
                    }
                    else
                    {
                        UIDispatcher.Invoke(taskAction);
                    }
                }
            }, _cancelTokenSrc.Token);
        }
        #endregion


        #region Props
        #region Standard Props
        /// <summary>
        /// Gets or sets the list of project setup paths.
        /// </summary>
        public PathItem[] ProjectSetups { get; set; }

        /// <summary>
        /// Gets or sets the window that will be the owner of any dialog windows.
        /// </summary>
        public Window DialogOwner { get; set; }

        /// <summary>
        /// Gets or sets the surface that the particles will render to.
        /// </summary>
        public Control RenderSurface { get; set; }

        /// <summary>
        /// Gets or sets the dispatcher of the UI thread.
        /// </summary>
        public Dispatcher UIDispatcher { get; set; }

        /// <summary>
        /// Gets or sets the width of the render surface.
        /// </summary>
        public int RenderSurfaceWidth
        {
            get => _graphicsEngine.Width;
            set
            {
                _graphicsEngine.Width = value;
            }
        }

        /// <summary>
        /// Gets or sets the height of the render surface.
        /// </summary>
        public int RenderSurfaceHeight
        {
            get => _graphicsEngine.Height;
            set
            {
                _graphicsEngine.Height = value;
            }
        }

        /// <summary>
        /// Gets or sets the total number of particles that can be alive at any time.
        /// </summary>
        public int TotalParticles
        {
            get => _graphicsEngine.ParticleEngine.TotalParticlesAliveAtOnce;
            set => _graphicsEngine.ParticleEngine.TotalParticlesAliveAtOnce = value;
        }

        /// <summary>
        /// Gets the total number of living particles.
        /// </summary>
        public int TotalLivingParticles => _graphicsEngine.ParticleEngine.TotalLivingParticles;

        /// <summary>
        /// Gets the total number of dead particles.
        /// </summary>
        public int TotalDeadParticles => _graphicsEngine.ParticleEngine.TotalDeadParticles;

        /// <summary>
        /// Gets or sets the minimum value of the red color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int RedMin
        {
            get => _graphicsEngine.ParticleEngine.RedMin;
            set
            {
                _graphicsEngine.ParticleEngine.RedMin = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of the red color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int RedMax
        {
            get => _graphicsEngine.ParticleEngine.RedMax;
            set
            {
                _graphicsEngine.ParticleEngine.RedMax = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the green color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int GreenMin
        {
            get => _graphicsEngine.ParticleEngine.GreenMin;
            set
            {
                _graphicsEngine.ParticleEngine.GreenMin = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of the green color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int GreenMax
        {
            get => _graphicsEngine.ParticleEngine.GreenMax;
            set
            {
                _graphicsEngine.ParticleEngine.GreenMax = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the blue color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int BlueMin
        {
            get => _graphicsEngine.ParticleEngine.BlueMin;
            set
            {
                _graphicsEngine.ParticleEngine.BlueMin = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of the blue color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int BlueMax
        {
            get => _graphicsEngine.ParticleEngine.BlueMax;
            set
            {
                _graphicsEngine.ParticleEngine.BlueMax = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum size of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float SizeMin
        {
            get => _graphicsEngine.ParticleEngine.SizeMin;
            set
            {
                _graphicsEngine.ParticleEngine.SizeMin = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum size of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float SizeMax
        {
            get => _graphicsEngine.ParticleEngine.SizeMax;
            set
            {
                _graphicsEngine.ParticleEngine.SizeMax = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum angle of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float AngleMin
        {
            get => _graphicsEngine.ParticleEngine.AngleMin;
            set => _graphicsEngine.ParticleEngine.AngleMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum angle of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float AngleMax
        {
            get => _graphicsEngine.ParticleEngine.AngleMax;
            set => _graphicsEngine.ParticleEngine.AngleMax = value;
        }

        /// <summary>
        /// Gets or sets the minimum angular velocity of the range that a <see cref="Particle"/> be will randomly set to.
        /// </summary>
        public float AngularVelocityMin
        {
            get => _graphicsEngine.ParticleEngine.AngularVelocityMin;
            set => _graphicsEngine.ParticleEngine.AngularVelocityMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum angular velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float AngularVelocityMax
        {
            get => _graphicsEngine.ParticleEngine.AngularVelocityMax;
            set => _graphicsEngine.ParticleEngine.AngularVelocityMax = value;
        }

        /// <summary>
        /// Gets or sets the minimum X velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float VelocityXMin
        {
            get => _graphicsEngine.ParticleEngine.VelocityXMin;
            set => _graphicsEngine.ParticleEngine.VelocityXMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum X velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float VelocityXMax
        {
            get => _graphicsEngine.ParticleEngine.VelocityXMax;
            set => _graphicsEngine.ParticleEngine.VelocityXMax = value;
        }

        /// <summary>
        /// Gets or sets the minimum X velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float VelocityYMin
        {
            get => _graphicsEngine.ParticleEngine.VelocityYMin;
            set => _graphicsEngine.ParticleEngine.VelocityYMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum Y velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float VelocityYMax
        {
            get => _graphicsEngine.ParticleEngine.VelocityYMax;
            set => _graphicsEngine.ParticleEngine.VelocityYMax = value;
        }

        /// <summary>
        /// Gets or sets the minimum life time of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int LifetimeMin
        {
            get => _graphicsEngine.ParticleEngine.LifeTimeMin;
            set => _graphicsEngine.ParticleEngine.LifeTimeMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum life time of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int LifetimeMax
        {
            get => _graphicsEngine.ParticleEngine.LifeTimeMax;
            set => _graphicsEngine.ParticleEngine.LifeTimeMax = value;
        }

        /// <summary>
        /// Gets or sets the minimum spawn rate of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int SpawnRateMin
        {
            get => _graphicsEngine.ParticleEngine.SpawnRateMin;
            set => _graphicsEngine.ParticleEngine.SpawnRateMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum spawn rate of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int SpawnRateMax
        {
            get => _graphicsEngine.ParticleEngine.SpawnRateMax;
            set => _graphicsEngine.ParticleEngine.SpawnRateMax = value;
        }

        /// <summary>
        /// Gets or sets a value indicating if the colors will be randomly chosen from a list.
        /// </summary>
        public bool UseColorsFromList
        {
            get => _graphicsEngine.ParticleEngine.UseTintColorList;
            set
            {
                _graphicsEngine.ParticleEngine.UseTintColorList = value;
            }
        }

        /// <summary>
        /// Gets or sets the list of colors to randomly choose from.
        /// </summary>
        public ObservableCollection<ColorItem> Colors
        {
            get
            {
                var result = new ObservableCollection<ColorItem>();

                for (int i = 0; i < _graphicsEngine.ParticleEngine.TintColors.Length; i++)
                {
                    var clrItem = _graphicsEngine.ParticleEngine.TintColors[i].ToColorItem();
                    clrItem.Id = i;

                    result.Add(clrItem);
                }


                return result;
            }
            set
            {
                var result = new List<GameColor>();

                foreach (var clr in value)
                {
                    result.Add(new GameColor(clr.ColorBrush.Color.R, clr.ColorBrush.Color.G, clr.ColorBrush.Color.B, clr.ColorBrush.Color.A));
                }

                _graphicsEngine.ParticleEngine.TintColors = result.ToArray();
            }
        }

        /// <summary>
        /// Gets the title of the window.
        /// </summary>
        public string WindowTitle => $"Particle Maker{(string.IsNullOrEmpty(CurrentOpenProject) ? "" : " - ")}{CurrentOpenProject}";

        /// <summary>
        /// Gets or sets the name of the currently open project.
        /// </summary>
        public string CurrentOpenProject { get; set; }
        #endregion


        #region Command Props
        /// <summary>
        /// Used for creating a new project when executed.
        /// </summary>
        public RelayCommand NewProject
        {
            get
            {
                if (_newProjectCommand == null)
                    _newProjectCommand = new RelayCommand(NewProjectExecute, (param) => true);


                return _newProjectCommand;
            }
        }

        /// <summary>
        /// Used for creating opening a project when executed.
        /// </summary>
        public RelayCommand OpenProject
        {
            get
            {
                if (_openProjectCommand == null)
                    _openProjectCommand = new RelayCommand(OpenProjectExecute, (param) => true);


                return _openProjectCommand;
            }
        }

        /// <summary>
        /// Gets the command that is invoked when a setup list item has been selected.
        /// </summary>
        public RelayCommand SetupItemSelected
        {
            get
            {
                if (_setupItemSelectedCommand == null)
                    _setupItemSelectedCommand = new RelayCommand(SetupItemSelectedExcute, (param) => true);


                return _setupItemSelectedCommand;
            }
        }

        /// <summary>
        /// Adds a setup to the currently loaded project.
        /// </summary>
        public RelayCommand AddSetup
        {
            get
            {
                if (_addSetupCommand == null)
                    _addSetupCommand = new RelayCommand(AddSetupExcute, (param) => true);


                return _addSetupCommand;
            }
        }

        /// <summary>
        /// Renames a project.
        /// </summary>
        public RelayCommand RenameProject
        {
            get
            {
                if (_renameProjectCommand == null)
                    _renameProjectCommand = new RelayCommand(RenameProjectExecute, (param) => true);


                return _renameProjectCommand;
            }
        }
        #endregion
        #endregion


        #region Public Methods
        /// <summary>
        /// Starts the graphics engine to start the rendering process.
        /// </summary>
        public void StartEngine()
        {
            _startupTask.Start();
            MainWindow.SetFocus();
        }


        /// <summary>
        /// Shuts down the graphics engine.
        /// </summary>
        public void ShutdownEngine()
        {
            _cancelTokenSrc.Cancel();

            _graphicsEngine.Stop();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Updates the <see cref="TotalLivingParticles"/> property to update the UI.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void _particleEngine_LivingParticlesCountChanged(object sender, EventArgs e)
        {
            NotifyPropChange(nameof(TotalLivingParticles));
            NotifyPropChange(nameof(TotalDeadParticles));
        }


        #region Command Execute Methods
        /// <summary>
        /// Creates a new project.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void NewProjectExecute(object parameter)
        {
            var msg = "Enter a new project name to create a project.  \nDuplicate project names not aloud.";

            var invalidProjNames = _projectManager.Projects;

            var inputDialog = new InputDialog("Create New Project", msg, "", _illegalCharacters, invalidProjNames)
            {
                Owner = DialogOwner
            };

            var dialogResult = inputDialog.ShowDialog();

            if (dialogResult == true)
            {
                try
                {
                    _projectManager.Create(inputDialog.InputValue);

                    CurrentOpenProject = inputDialog.InputValue;

                    NotifyPropChange(nameof(WindowTitle));
                }
                catch (Exception ex)
                {
                    //TODO: Use custom exception handler class here.
                    WPFMsgBox.Show(ex.Message);
                }
            }
        }


        /// <summary>
        /// Opens a project.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void OpenProjectExecute(object param)
        {
            var projectListDialog = new ProjectListDialog("Open Project")
            {
                Owner = DialogOwner,
                ProjectPaths = _projectManager.ProjectPaths
            };

            var dialogResult = projectListDialog.ShowDialog();

            if (dialogResult == true)
            {
                CurrentOpenProject = projectListDialog.SelectedProject;

                _projectSettings = _projectSettingsManager.Load(CurrentOpenProject);

                ProjectSetups = (from path in _setupManager.GetSetupPaths(CurrentOpenProject)
                                 select new PathItem()
                                 {
                                     FilePath = path
                                 }).ToArray();

                NotifyPropChange(nameof(ProjectSetups));
                NotifyPropChange(nameof(WindowTitle));
            }
        }


        /// <summary>
        /// Adds a new setup to the currently open project.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void AddSetupExcute(object param)
        {
            var setupName = param as string;

            if (string.IsNullOrEmpty(setupName))
                return;

            _setupManager.Create(CurrentOpenProject, setupName);

            //Update the list with the newly created setup
            ProjectSetups = (from s in _setupManager.GetSetupPaths(CurrentOpenProject)
                             select new PathItem() { FilePath = s }).ToArray();

            NotifyPropChange(nameof(ProjectSetups));
        }


        /// <summary>
        /// Loads a selected setup and starts the particle engine.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void SetupItemSelectedExcute(object param)
        {
            var eventArgs = param as SetupItemEventArgs;

            if (eventArgs == null)
                return;

            var setupData = _setupManager.Load(_projectSettings.ProjectName, eventArgs.Name);
        }


        /// <summary>
        /// Renames a project.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void RenameProjectExecute(object param)
        {
            var projectListDialog = new ProjectListDialog("Select Project To Rename")
            {
                Owner = DialogOwner,
                ProjectPaths = _projectManager.ProjectPaths
            };

            var projectListDialogResult = projectListDialog.ShowDialog();

            if (projectListDialogResult == true)
            {
                var invalidProjNames = _projectManager.Projects;

                var inputDialog = new InputDialog("Rename Project", "Enter new project name.", invalidChars: _illegalCharacters, invalidValues: invalidProjNames)
                {
                    Owner = DialogOwner
                };

                var inputDialogResult = inputDialog.ShowDialog();

                if (inputDialogResult == true)
                {
                    //Update the project name value in the file itself
                    var projectSettings = _projectSettingsManager.Load(projectListDialog.SelectedProject);
                    projectSettings.ProjectName = inputDialog.InputValue;
                    _projectSettingsManager.Save(projectListDialog.SelectedProject, projectSettings);

                    _projectSettingsManager.Rename(projectListDialog.SelectedProject, inputDialog.InputValue);

                    _projectManager.Rename(projectListDialog.SelectedProject, inputDialog.InputValue);

                    //If the project that just got renamed is opened
                    if (!string.IsNullOrEmpty(CurrentOpenProject) && CurrentOpenProject == projectListDialog.SelectedProject)
                    {
                        //Update the currently open project value
                        CurrentOpenProject = inputDialog.InputValue;

                        ProjectSetups = (from path in _setupManager.GetSetupPaths(CurrentOpenProject)
                                         select new PathItem()
                                         {
                                             FilePath = path
                                         }).ToArray();

                        NotifyPropChange(nameof(ProjectSetups));
                        NotifyPropChange(nameof(WindowTitle));
                    }
                }
            }
        }
        #endregion


        /// <summary>
        /// Notifies the binding system that a property value has changed.
        /// </summary>
        /// <param name="propName"></param>
        [ExcludeFromCodeCoverage]
        private void NotifyPropChange([CallerMemberName]string propName = "")
        {
            if (!string.IsNullOrEmpty(propName))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
