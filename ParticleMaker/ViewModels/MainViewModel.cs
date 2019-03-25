using System;
using System.Windows.Forms;
using System.Windows.Threading;
using KDParticleEngine;
using CoreVector = KDScorpionCore.Vector;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using KDScorpionCore.Graphics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Threading;
using ParticleMaker.Management;
using ParticleMaker.Dialogs;
using System.Windows;
using ParticleMaker.UserControls;
using System.Linq;
using ParticleMaker.Services;
using ParticleMaker.CustomEventArgs;
using ParticleMaker.Exceptions;
using WPFMsgBox = System.Windows.MessageBox;
using FolderDialog = System.Windows.Forms.FolderBrowserDialog;
using FolderDialogResult = System.Windows.Forms.DialogResult;
using System.ComponentModel;

namespace ParticleMaker.ViewModels
{
    /// <summary>
    /// The main view model for the application.
    /// </summary>
    public class MainViewModel : ViewModel
    {
        #region Private Fields
        private readonly char[] _illegalCharacters = new[] { '\\', '/', ':', '*', '?', '\"', '<', '>', '|', '.' };
        private readonly GraphicsEngine _graphicsEngine;
        private readonly ProjectManager _projectManager;
        private readonly ProjectSettingsManager _projectSettingsManager;
        private readonly SetupManager _setupManager;
        private readonly SetupDeployService _setupDeployService;
        private readonly ParticleManager _particleManager;
        private CancellationTokenSource _cancelTokenSrc;
        private Task _startupTask;
        private RelayCommand _newProjectCommand;
        private RelayCommand _openProjectCommand;
        private ProjectSettings _projectSettings;
        private RelayCommand _setupItemSelectedCommand;
        private RelayCommand _addSetupCommand;
        private RelayCommand _renameProjectCommand;
        private RelayCommand _pauseCommand;
        private RelayCommand _playCommand;
        private RelayCommand _saveSetupCommand;
        private RelayCommand _updateDeploymentPathCommand;
        private RelayCommand _deploySetupCommand;
        private RelayCommand _deleteProjectCommand;
        private RelayCommand _renameSetupCommand;
        private RelayCommand _deleteSetupCommand;
        private RelayCommand _addParticleCommand;
        private RelayCommand _renameParticleCommand;
        private RelayCommand _deleteParticleCommand;
        private string _currentOpenProject;
        #endregion


        #region Constructor
        /// <summary>
        /// Creates a new instance of <see cref="MainViewModel"/>.
        /// </summary>
        /// <param name="graphicsEngine">The graphics engine used to perform and drive the rendering.</param>
        /// <param name="projectManager">Manages the project.</param>
        /// <param name="projectSettingsManager">Manages all of the project related settings.</param>
        /// <param name="setupManager">Manages the setups within a project.</param>
        /// <param name="setupDeployService">The service responsible for deploying setups.</param>
        [ExcludeFromCodeCoverage]
        public MainViewModel(GraphicsEngine graphicsEngine, ProjectManager projectManager,
            ProjectSettingsManager projectSettingsManager, SetupManager setupManager,
            SetupDeployService setupDeployService, ParticleManager particleManager)
        {
            _graphicsEngine = graphicsEngine;
            _projectManager = projectManager;
            _projectSettingsManager = projectSettingsManager;
            _setupManager = setupManager;
            _setupDeployService = setupDeployService;
            _particleManager = particleManager;
        }
        #endregion


        #region Props
        #region Standard Props
        /// <summary>
        /// Gets a value indicating if any of the setup changes have changed.
        /// </summary>
        public bool SettingsChanged { get; private set; } = false;

        /// <summary>
        /// Gets or sets the list of project setup paths.
        /// </summary>
        public PathItem[] ProjectSetups { get; set; }

        /// <summary>
        /// Gets or sets the list of setup particles.
        /// </summary>
        public PathItem[] Particles { get; set; }

        /// <summary>
        /// Gets or sets the window that will be the owner of any dialog windows.
        /// </summary>
        [ExcludeFromCodeCoverage]
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

                _graphicsEngine.Pause();
                _graphicsEngine.Play();

                NotifyPropChange();
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
                _graphicsEngine.Pause();

                _graphicsEngine.Height = value;

                _graphicsEngine.Play();

                NotifyPropChange();
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
                SettingsChanged = true;
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
                SettingsChanged = true;
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
                SettingsChanged = true;
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
                SettingsChanged = true;
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
                SettingsChanged = true;
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
                SettingsChanged = true;
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
                SettingsChanged = true;
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
                SettingsChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets the minimum angle of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float AngleMin
        {
            get => _graphicsEngine.ParticleEngine.AngleMin;
            set
            {
                _graphicsEngine.ParticleEngine.AngleMin = value;
                SettingsChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets the maximum angle of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float AngleMax
        {
            get => _graphicsEngine.ParticleEngine.AngleMax;
            set
            {
                _graphicsEngine.ParticleEngine.AngleMax = value;
                SettingsChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets the minimum angular velocity of the range that a <see cref="Particle"/> be will randomly set to.
        /// </summary>
        public float AngularVelocityMin
        {
            get => _graphicsEngine.ParticleEngine.AngularVelocityMin;
            set
            {
                _graphicsEngine.ParticleEngine.AngularVelocityMin = value;
                SettingsChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets the maximum angular velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float AngularVelocityMax
        {
            get => _graphicsEngine.ParticleEngine.AngularVelocityMax;
            set
            {
                _graphicsEngine.ParticleEngine.AngularVelocityMax = value;
                SettingsChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets the minimum X velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float VelocityXMin
        {
            get => _graphicsEngine.ParticleEngine.VelocityXMin;
            set
            {
                _graphicsEngine.ParticleEngine.VelocityXMin = value;
                SettingsChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets the maximum X velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float VelocityXMax
        {
            get => _graphicsEngine.ParticleEngine.VelocityXMax;
            set
            {
                _graphicsEngine.ParticleEngine.VelocityXMax = value;
                SettingsChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets the minimum X velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float VelocityYMin
        {
            get => _graphicsEngine.ParticleEngine.VelocityYMin;
            set
            {
                _graphicsEngine.ParticleEngine.VelocityYMin = value;
                SettingsChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets the maximum Y velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float VelocityYMax
        {
            get => _graphicsEngine.ParticleEngine.VelocityYMax;
            set
            {
                _graphicsEngine.ParticleEngine.VelocityYMax = value;
                SettingsChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets the minimum life time of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int LifetimeMin
        {
            get => _graphicsEngine.ParticleEngine.LifeTimeMin;
            set
            {
                _graphicsEngine.ParticleEngine.LifeTimeMin = value;
                SettingsChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets the maximum life time of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int LifetimeMax
        {
            get => _graphicsEngine.ParticleEngine.LifeTimeMax;
            set
            {
                _graphicsEngine.ParticleEngine.LifeTimeMax = value;
                SettingsChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets the minimum spawn rate of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int SpawnRateMin
        {
            get => _graphicsEngine.ParticleEngine.SpawnRateMin;
            set
            {
                _graphicsEngine.ParticleEngine.SpawnRateMin = value;
                SettingsChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets the maximum spawn rate of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int SpawnRateMax
        {
            get => _graphicsEngine.ParticleEngine.SpawnRateMax;
            set
            {
                _graphicsEngine.ParticleEngine.SpawnRateMax = value;
                SettingsChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the colors will be randomly chosen from a list.
        /// </summary>
        public bool UseColorsFromList
        {
            get => _graphicsEngine.ParticleEngine.UseColorsFromList;
            set
            {
                _graphicsEngine.ParticleEngine.UseColorsFromList = value;
                SettingsChanged = true;
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
                    result.Add(new GameColor(clr.ColorBrush.Color.A, clr.ColorBrush.Color.R, clr.ColorBrush.Color.G, clr.ColorBrush.Color.B));
                }

                _graphicsEngine.ParticleEngine.TintColors = result.ToArray();
                SettingsChanged = true;
            }
        }

        /// <summary>
        /// Gets the title of the window.
        /// </summary>
        public string WindowTitle => $"Particle Maker{(string.IsNullOrEmpty(CurrentOpenProject) ? "" : " - ")}{CurrentOpenProject}";

        /// <summary>
        /// Gets or sets the name of the currently open project.
        /// </summary>
        public string CurrentOpenProject
        {
            get => _currentOpenProject;
            set
            {
                _currentOpenProject = value;

                NotifyPropChange();
            }
        }

        /// <summary>
        /// Gets or sets the currently loaded setup.
        /// </summary>
        public string CurrentLoadedSetup { get; set; }

        /// <summary>
        /// Gets or sets the path of where the currently loaded setup will be deployed to.
        /// </summary>
        public string SetupDeploymentPath { get; set; }
        #endregion


        #region Command Props
        /// <summary>
        /// Gets the command that is invoked when a setup list item has been selected.
        /// </summary>
        public RelayCommand SetupItemSelected
        {
            get
            {
                if (_setupItemSelectedCommand == null)
                    _setupItemSelectedCommand = new RelayCommand(SetupItemSelectedExecute, (param) => true);


                return _setupItemSelectedCommand;
            }
        }

        /// <summary>
        /// Gets the command that will play the particle rendering.
        /// </summary>
        public RelayCommand Play
        {
            get
            {
                if (_playCommand == null)
                    _playCommand = new RelayCommand(PlayExecute, (param) => true);


                return _playCommand;
            }
        }

        /// <summary>
        /// Gets the command that will pause the particle rendering.
        /// </summary>
        public RelayCommand Pause
        {
            get
            {
                if (_pauseCommand == null)
                    _pauseCommand = new RelayCommand(PauseExecute, (param) => true);


                return _pauseCommand;
            }
        }

        /// <summary>
        /// Gets the command that is used for creating a new project.
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
        /// Ges the command that is used for opening a project.
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

        /// <summary>
        /// Deletes a project.
        /// </summary>
        public RelayCommand DeleteProject
        {
            get
            {
                if (_deleteProjectCommand == null)
                    _deleteProjectCommand = new RelayCommand(DeleteProjectExecute, (param) => true);


                return _deleteProjectCommand;
            }
        }

        /// <summary>
        /// Gets the command that adds a setup to the currently loaded project.
        /// </summary>
        public RelayCommand AddSetup
        {
            get
            {
                if (_addSetupCommand == null)
                    _addSetupCommand = new RelayCommand(AddSetupExecute, (param) => true);


                return _addSetupCommand;
            }
        }

        /// <summary>
        /// Gets the command that saves the current setup.
        /// </summary>
        public RelayCommand SaveSetup
        {
            get
            {
                if (_saveSetupCommand == null)
                    _saveSetupCommand = new RelayCommand(SaveSetupExecute, (param) => true);


                return _saveSetupCommand;
            }
        }

        /// <summary>
        /// Deploys the setup to the set deployment path.
        /// </summary>
        public RelayCommand DeploySetup
        {
            get
            {
                if (_deploySetupCommand == null)
                    _deploySetupCommand = new RelayCommand(DeploySetupExecute, (param) => true);


                return _deploySetupCommand;
            }
        }

        /// <summary>
        /// Renames a setup.
        /// </summary>
        public RelayCommand RenameSetup
        {
            get
            {
                if (_renameSetupCommand == null)
                    _renameSetupCommand = new RelayCommand(RenameSetupExecute, (param) => true);


                return _renameSetupCommand;
            }
        }

        /// <summary>
        /// Updates the deployment path for a setup.
        /// </summary>
        public RelayCommand UpdateDeploymentPath
        {
            get
            {
                if (_updateDeploymentPathCommand == null)
                    _updateDeploymentPathCommand = new RelayCommand(UpdateDeployPathExecute, (param) => true);


                return _updateDeploymentPathCommand;
            }
        }

        /// <summary>
        /// Deletes a setup from a project.
        /// </summary>
        public RelayCommand DeleteSetup
        {
            get
            {
                if (_deleteSetupCommand == null)
                    _deleteSetupCommand = new RelayCommand(DeleteSetupExecute, (param) => true);


                return _deleteSetupCommand;
            }
        }

        /// <summary>
        /// Adds a particle to a selected project setup.
        /// </summary>
        public RelayCommand AddParticle
        {
            get
            {
                if (_addParticleCommand == null)
                    _addParticleCommand = new RelayCommand(AddParticleExecute, (param) => true);


                return _addParticleCommand;
            }
        }

        /// <summary>
        /// Renames a particle in a project setup.
        /// </summary>
        public RelayCommand RenameParticle
        {
            get
            {
                if (_renameParticleCommand == null)
                    _renameParticleCommand = new RelayCommand(RenameParticleExecute, (param) => true);


                return _renameParticleCommand;
            }
        }

        /// <summary>
        /// Deletes a particle in a project setup.
        /// </summary>
        public RelayCommand DeleteParticle
        {
            get
            {
                if (_deleteParticleCommand == null)
                    _deleteParticleCommand = new RelayCommand(DeleteParticleExecute, (param) => true);


                return _deleteParticleCommand;
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
                            _graphicsEngine.Start();
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

            _startupTask.Start();

            MainWindow.SetFocus();
        }


        /// <summary>
        /// Shuts down the graphics engine.
        /// </summary>
        public void ShutdownEngine()
        {
            _cancelTokenSrc?.Cancel();

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


        /// <summary>
        /// Updates the particle list.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void UpdateParticleList()
        {
            //Get the list of all the particles
            Particles = (from p in _particleManager.GetParticlePaths(CurrentOpenProject, CurrentLoadedSetup)
                         select new PathItem()
                         {
                             FilePath = p
                         }).ToArray();

            _graphicsEngine.TexturePaths = (from p in Particles select p.FilePath).ToArray();

            NotifyPropChange(nameof(Particles));
        }


        #region Command Execute Methods
        /// <summary>
        /// Plays the particle rendering.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void PlayExecute(object param)
        {
            _graphicsEngine.Play();
        }


        /// <summary>
        /// Pauses the particle rendering.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void PauseExecute(object param)
        {
            _graphicsEngine.Pause();
        }


        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void NewProjectExecute(object param)
        {
            try
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

                        _projectSettings = _projectSettingsManager.Load(inputDialog.InputValue);

                        NotifyPropChange(nameof(WindowTitle));
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.Handle(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }


        /// <summary>
        /// Opens a project.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void OpenProjectExecute(object param)
        {
            try
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
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }


        /// <summary>
        /// Renames a project.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void RenameProjectExecute(object param)
        {
            try
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
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }


        /// <summary>
        /// Deletes a project.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void DeleteProjectExecute(object param)
        {
            try
            {
                var projListDialog = new ProjectListDialog("Delete Project")
                {
                    Owner = DialogOwner,
                    ProjectPaths = _projectManager.ProjectPaths
                };

                if (projListDialog.ShowDialog() == true)
                {
                    var msg = $"Are you sure you want to delete the project named '{projListDialog.SelectedProject}'";

                    //Ask user if they are sure they want to delete the project
                    if (WPFMsgBox.Show(DialogOwner, msg, "Delete Project?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        //If the project to be deleted is currently loaded, unload the project
                        if (CurrentOpenProject == projListDialog.SelectedProject)
                        {
                            CurrentOpenProject = string.Empty;
                            CurrentLoadedSetup = string.Empty;
                            ProjectSetups = null;
                            SetupDeploymentPath = string.Empty;

                            _graphicsEngine.Pause(true);
                        }

                        _projectManager.Delete(projListDialog.SelectedProject);

                        NotifyAllPropChanges(this.GetPropertyNames());
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }


        /// <summary>
        /// Adds a new setup to the currently open project.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void AddSetupExecute(object param)
        {
            try
            {
                var setupName = param as string;

                if (string.IsNullOrEmpty(setupName))
                    return;

                _setupManager.Create(CurrentOpenProject, setupName);

                var projSettings = _projectSettingsManager.Load(CurrentOpenProject);

                var deploySettings = projSettings.SetupDeploySettings.ToList();

                deploySettings.Add(new DeploymentSetting()
                {
                    SetupName = setupName,
                    DeployPath = string.Empty
                });

                projSettings.SetupDeploySettings = deploySettings.ToArray();

                _projectSettingsManager.Save(CurrentOpenProject, projSettings);

                //Update the list with the newly created setup
                ProjectSetups = (from s in _setupManager.GetSetupPaths(CurrentOpenProject)
                                 select new PathItem() { FilePath = s }).ToArray();

                NotifyPropChange(nameof(ProjectSetups));
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }


        /// <summary>
        /// Saves the current setup by using the the given <paramref name="param"/> as a name.
        /// </summary>
        /// <param name="param">The param that is holding the setup to save.</param>
        [ExcludeFromCodeCoverage]
        private void SaveSetupExecute(object param)
        {
            try
            {
                var setupToSave = _graphicsEngine.ParticleEngine.GenerateParticleSetup();

                _setupManager.Save(CurrentOpenProject, CurrentLoadedSetup, setupToSave);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }


        /// <summary>
        /// Deploys the setup to the location set by the deployment path.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void DeploySetupExecute(object param)
        {
            if (!(param is DeploySetupEventArgs eventArgs))
                throw new InvalidCommandActionParamTypeException(nameof(DeploySetupExecute), nameof(param));

            _setupDeployService.Deploy(CurrentOpenProject, CurrentLoadedSetup, eventArgs.DeploymentPath);
        }


        /// <summary>
        /// Renames a setup.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void RenameSetupExecute(object param)
        {
            if (!(param is RenameItemEventArgs eventArgs))
                throw new InvalidCommandActionParamTypeException(nameof(RenameSetupExecute), nameof(param));

            var dialogResult = WPFMsgBox.Show(DialogOwner, $"Are you sure you want to rename '{eventArgs.OldName}' to '{eventArgs.NewName}'?", "Rename Setup", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (dialogResult == MessageBoxResult.Yes)
            {
                //Update the name of the setup in the list of setup deployment settings
                _projectSettingsManager.RenameDeploymentSetupName(CurrentOpenProject, eventArgs.OldName, eventArgs.NewName);

                //Rename the setup folder and setup json file
                _setupManager.Rename(CurrentOpenProject, eventArgs.OldName, eventArgs.NewName);

                ProjectSetups = (from p in _setupManager.GetSetupPaths(CurrentOpenProject)
                                 select new PathItem() { FilePath = p }).ToArray();

                NotifyPropChange(nameof(ProjectSetups));
            }
        }


        /// <summary>
        /// Loads a selected setup and starts the particle engine.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void SetupItemSelectedExecute(object param)
        {
            try
            {
                if (!(param is string setupName))
                    return;

                _graphicsEngine.Pause();

                var setupData = _setupManager.Load(_projectSettings.ProjectName, setupName);
                _graphicsEngine.ParticleEngine.ApplySetup(setupData);

                CurrentLoadedSetup = setupName;

                //Load the deployment path for the currently loaded setup
                var projSettings = _projectSettingsManager.Load(CurrentOpenProject);

                var deployPath = (from s in projSettings.SetupDeploySettings where s.SetupName == CurrentLoadedSetup select s.DeployPath).FirstOrDefault();

                SetupDeploymentPath = deployPath;

                UpdateParticleList();

                _graphicsEngine.TexturePaths = (from p in Particles select p.FilePath).ToArray();

                var propNames = setupData.GetPropertyNames().ToList();

                propNames.Add(nameof(SetupDeploymentPath));

                NotifyAllPropChanges(propNames.ToArray());

                NotifyPropChange(nameof(CurrentLoadedSetup));

                _graphicsEngine.Play();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }


        /// <summary>
        /// Updates the deployment path for the currently loaded setup.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void UpdateDeployPathExecute(object param)
        {
            try
            {
                if (!(param is DeploySetupEventArgs eventArgs))
                    throw new ArgumentException($"The parameter in method '{nameof(UpdateDeployPathExecute)}' must be of type '{nameof(DeploySetupEventArgs)}' for the command to execute.");

                //Updates the deployment path for the setup
                var projSettings = _projectSettingsManager.Load(CurrentOpenProject);

                var deploySettings = projSettings.SetupDeploySettings.ToList();

                var deploySetting = (from s in deploySettings
                                     where s.SetupName == CurrentLoadedSetup
                                     select s).FirstOrDefault();

                deploySetting.DeployPath = eventArgs.DeploymentPath;

                deploySettings.Remove(deploySetting);

                deploySettings.Add(deploySetting);

                projSettings.SetupDeploySettings = deploySettings.ToArray();

                _projectSettingsManager.Save(CurrentOpenProject, projSettings);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }


        /// <summary>
        /// Deletes a setup from the currently loaded project.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void DeleteSetupExecute(object param)
        {
            if (!(param is ItemEventArgs eventArgs))
                throw new InvalidCommandActionParamTypeException(nameof(DeleteSetupExecute), nameof(param));

            var dialogResult = WPFMsgBox.Show(DialogOwner, $"Are you sure you want to delete the setup '{eventArgs.Name}'?", "Delete Setup", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (dialogResult == MessageBoxResult.Yes)
            {
                _setupManager.Delete(CurrentOpenProject, eventArgs.Name);

                ProjectSetups = (from p in _setupManager.GetSetupPaths(CurrentOpenProject)
                                 select new PathItem() { FilePath = p }).ToArray();

                NotifyPropChange(nameof(ProjectSetups));
            }
        }


        /// <summary>
        /// Adds a particle to a setup in a project.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void AddParticleExecute(object param)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Title = "Choose particle to add . . .",
                InitialDirectory = @"C:\"
            };

            
            if (openFileDialog.ShowDialog() == FolderDialogResult.OK)
            {
                _particleManager.AddParticle(CurrentOpenProject, CurrentLoadedSetup, openFileDialog.FileName, true);

                //Get the list of all the particles
                Particles = (from p in _particleManager.GetParticlePaths(CurrentOpenProject, CurrentLoadedSetup)
                             select new PathItem()
                             {
                                 FilePath = p
                             }).ToArray();

                _graphicsEngine.Pause();

                _graphicsEngine.TexturePaths = (from p in Particles select p.FilePath).ToArray();

                _graphicsEngine.Play();

                NotifyPropChange(nameof(Particles));
            }
        }


        /// <summary>
        /// Renames a particle in a project setup.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void RenameParticleExecute(object param)
        {
            if (!(param is RenameItemEventArgs eventArgs))
                throw new InvalidCommandActionParamTypeException(nameof(RenameParticleExecute), nameof(param));

            if (WPFMsgBox.Show($"Are you sure you want to rename the particle '{eventArgs.OldName}' to '{eventArgs.NewName}'?", "Rename Particle", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _graphicsEngine.Pause();

                _particleManager.RenameParticle(CurrentOpenProject, CurrentLoadedSetup, eventArgs.OldName, eventArgs.NewName);

                UpdateParticleList();

                _graphicsEngine.Play();
            }
        }


        /// <summary>
        /// Deletes a particle in a project setup.
        /// </summary>
        /// <param name="param">The incoming data upon execution of the <see cref="ICommand"/>.</param>
        [ExcludeFromCodeCoverage]
        private void DeleteParticleExecute(object param)
        {
            if (!(param is ItemEventArgs eventArgs))
                throw new InvalidCommandActionParamTypeException(nameof(DeleteParticleExecute), nameof(param));

            //Confirm with the user
            if (WPFMsgBox.Show($"Are you sure you want to delete the particle named '{eventArgs.Name}'?", "Delete Particle", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _graphicsEngine.Pause();

                _particleManager.DeleteParticle(CurrentOpenProject, CurrentLoadedSetup, eventArgs.Name);

                UpdateParticleList();

                _graphicsEngine.Play();
            }
        }
        #endregion
        #endregion
    }
}
