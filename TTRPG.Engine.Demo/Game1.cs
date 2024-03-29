using System;
using System.Collections.Generic;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Effects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTRPG.Engine.Data;
using TTRPG.Engine.Demo.Views;
using TTRPG.Engine.Engine;
using TTRPG.Engine.Roles;

namespace TTRPG.Engine.Demo2;

public class SurvivalGame : Game
{
	private readonly GraphicsDeviceManager _graphics;
	private MonoGameEngine _gameEngine;

	private UIRoot _mainScreen;

	public SurvivalGame()
	{
		_graphics = new GraphicsDeviceManager(this);
		_graphics.PreparingDeviceSettings += (sender, e) =>
		{
			e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.One;  // 60fps
		};
		_graphics.PreferredBackBufferWidth = 800;
		_graphics.PreferredBackBufferHeight = 640;
		_graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
		_graphics.DeviceCreated += graphics_DeviceCreated;
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
	{
		_graphics.PreferMultiSampling = true;
		_graphics.GraphicsProfile = GraphicsProfile.HiDef;
		_graphics.SynchronizeWithVerticalRetrace = true;
		_graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
		e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 8;
	}

	void graphics_DeviceCreated(object sender, EventArgs e)
	{
		_gameEngine = new MonoGameEngine(GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
	}

	protected override void Initialize() => base.Initialize();

	protected override void LoadContent()
	{
		// load TTRPG Engine Services
		var collection = new ServiceCollection();
		collection.AddSingleton(new AutomaticCommandFactoryOptions
		{
			AutomaticCommands = new List<AutomaticCommand>
			{
				new SequenceAutoCommand
				{
					SequenceCategory = "Temporal",
					Command = "AdvanceTime",
					EntityFilter = r => r.Name == "Time",
					AliasEntitiesAs = "time",
					DefaultInputs = new Dictionary<string, string>{ { "elapsed", "15"} }
				},
				new SequenceAutoCommand
				{
					SequenceCategory = "Temporal",
					Command = "Grow",
					EntityFilter = r => r.Categories.Contains("Grows"),
					AliasEntitiesAs = "grower",
					DefaultInputs = new Dictionary<string, string>{ { "elapsed", "1"} }
				},
				new SequenceAutoCommand
				{
					SequenceCategory = "Temporal",
					Command = "Eat",
					EntityFilter = r => r.Categories.Contains("Biological"),
					AliasEntitiesAs = "eater",
					DefaultInputs = new Dictionary<string, string>{ { "elapsed", "1" } }
				},
				new AutomaticCommand
				{
					Command = "Bury",
					EntityFilter = r => r.Categories.Contains("Mortal") && double.Parse(r.Attributes["HP"]) <= 0
				}
			}
		});
		collection.AddTTRPGEngineServices();
		collection.AddTTRPGEngineDataLayer(new TTRPGEngineDataOptions
		{
			StorageType = DataStorageType.JsonFile,
			JsonFileStorageOptions = new JsonFileStorageOptions
			{
				EntitiesFileDirectory = "DataFiles/Entities",
				SequencesFileDirectory = "DataFiles/Sequences",
				MessageTemplatesDirectory = "DataFiles/MessageTemplates",
				RolesFileDirectory = "DataFiles/Roles",
				CommandsDirectory = "DataFiles/Commands"
			}
		});
		var provider = collection.BuildServiceProvider();
		var gameObject = provider.GetRequiredService<GameObject>();
		var engine = provider.GetRequiredService<TTRPGEngine>();
		gameObject.LoadAsync(engine).GetAwaiter().GetResult();
		var roleService = provider.GetRequiredService<IRoleService>();

		// TODO: use this.Content to load your game content here
		FontManager.Instance.LoadFonts(Content);
		SpriteFont font = Content.Load<SpriteFont>("Arial");
		FontManager.DefaultFont = _gameEngine.Renderer.CreateFont(font);

		ImageManager.Instance.LoadImages(Content);
		SoundManager.Instance.LoadSounds(Content);
		EffectManager.Instance.LoadEffects(Content);

		_mainScreen = new MainScreenViewPage(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
		_mainScreen.DataContext = new MainScreenView(gameObject, engine);

		// input bindings
		RelayCommand command = new RelayCommand(new Action<object>(o => Exit()));
		KeyBinding exitBinding = new KeyBinding(command, KeyCode.Escape, ModifierKeys.None);
		_mainScreen.InputBindings.Add(exitBinding);
	}

	protected override void UnloadContent()
	{
		// TODO: use this.Content to unload game content here
	}

	protected override void Update(GameTime gameTime)
	{
		_mainScreen.UpdateInput(gameTime.ElapsedGameTime.TotalMilliseconds);
		_mainScreen.UpdateLayout(gameTime.ElapsedGameTime.TotalMilliseconds);

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.Black);

		// TODO: Add your drawing code here
		_mainScreen.Draw(gameTime.ElapsedGameTime.TotalMilliseconds);

		base.Draw(gameTime);
	}
}
