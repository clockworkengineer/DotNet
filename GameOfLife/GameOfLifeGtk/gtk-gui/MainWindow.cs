
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.Fixed GameOfLifeFixed;

	private global::Gtk.Button StartButton;

	private global::Gtk.Button StopButton;

	private global::Gtk.Button ResetButton;

	private global::Gtk.Button QuitButton;

	private global::Gtk.Label GameOfLifeTickCount;

	private global::Gtk.Label GameOfLifeTickCountLabel;

	private global::Gtk.DrawingArea GameOfLifeCellGrid;

	protected virtual void Build()
	{
		global::Stetic.Gui.Initialize(this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString("Game Of Life");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.GameOfLifeFixed = new global::Gtk.Fixed();
		this.GameOfLifeFixed.Name = "GameOfLifeFixed";
		this.GameOfLifeFixed.HasWindow = false;
		// Container child GameOfLifeFixed.Gtk.Fixed+FixedChild
		this.StartButton = new global::Gtk.Button();
		this.StartButton.CanFocus = true;
		this.StartButton.Name = "StartButton";
		this.StartButton.UseUnderline = true;
		this.StartButton.Label = global::Mono.Unix.Catalog.GetString("Start");
		this.GameOfLifeFixed.Add(this.StartButton);
		global::Gtk.Fixed.FixedChild w1 = ((global::Gtk.Fixed.FixedChild)(this.GameOfLifeFixed[this.StartButton]));
		w1.X = 17;
		w1.Y = 23;
		// Container child GameOfLifeFixed.Gtk.Fixed+FixedChild
		this.StopButton = new global::Gtk.Button();
		this.StopButton.CanFocus = true;
		this.StopButton.Name = "StopButton";
		this.StopButton.UseUnderline = true;
		this.StopButton.Label = global::Mono.Unix.Catalog.GetString("Stop");
		this.GameOfLifeFixed.Add(this.StopButton);
		global::Gtk.Fixed.FixedChild w2 = ((global::Gtk.Fixed.FixedChild)(this.GameOfLifeFixed[this.StopButton]));
		w2.X = 19;
		w2.Y = 62;
		// Container child GameOfLifeFixed.Gtk.Fixed+FixedChild
		this.ResetButton = new global::Gtk.Button();
		this.ResetButton.CanFocus = true;
		this.ResetButton.Name = "ResetButton";
		this.ResetButton.UseUnderline = true;
		this.ResetButton.Label = global::Mono.Unix.Catalog.GetString("Reset");
		this.GameOfLifeFixed.Add(this.ResetButton);
		global::Gtk.Fixed.FixedChild w3 = ((global::Gtk.Fixed.FixedChild)(this.GameOfLifeFixed[this.ResetButton]));
		w3.X = 17;
		w3.Y = 104;
		// Container child GameOfLifeFixed.Gtk.Fixed+FixedChild
		this.QuitButton = new global::Gtk.Button();
		this.QuitButton.CanFocus = true;
		this.QuitButton.Name = "QuitButton";
		this.QuitButton.UseUnderline = true;
		this.QuitButton.Label = global::Mono.Unix.Catalog.GetString("Quit");
		this.GameOfLifeFixed.Add(this.QuitButton);
		global::Gtk.Fixed.FixedChild w4 = ((global::Gtk.Fixed.FixedChild)(this.GameOfLifeFixed[this.QuitButton]));
		w4.X = 20;
		w4.Y = 145;
		// Container child GameOfLifeFixed.Gtk.Fixed+FixedChild
		this.GameOfLifeTickCount = new global::Gtk.Label();
		this.GameOfLifeTickCount.Name = "GameOfLifeTickCount";
		this.GameOfLifeTickCount.LabelProp = global::Mono.Unix.Catalog.GetString("0");
		this.GameOfLifeFixed.Add(this.GameOfLifeTickCount);
		global::Gtk.Fixed.FixedChild w5 = ((global::Gtk.Fixed.FixedChild)(this.GameOfLifeFixed[this.GameOfLifeTickCount]));
		w5.X = 456;
		w5.Y = 445;
		// Container child GameOfLifeFixed.Gtk.Fixed+FixedChild
		this.GameOfLifeTickCountLabel = new global::Gtk.Label();
		this.GameOfLifeTickCountLabel.Name = "GameOfLifeTickCountLabel";
		this.GameOfLifeTickCountLabel.LabelProp = global::Mono.Unix.Catalog.GetString("Tick Count:");
		this.GameOfLifeFixed.Add(this.GameOfLifeTickCountLabel);
		global::Gtk.Fixed.FixedChild w6 = ((global::Gtk.Fixed.FixedChild)(this.GameOfLifeFixed[this.GameOfLifeTickCountLabel]));
		w6.X = 350;
		w6.Y = 445;
		// Container child GameOfLifeFixed.Gtk.Fixed+FixedChild
		this.GameOfLifeCellGrid = new global::Gtk.DrawingArea();
		this.GameOfLifeCellGrid.WidthRequest = 400;
		this.GameOfLifeCellGrid.HeightRequest = 400;
		this.GameOfLifeCellGrid.Name = "GameOfLifeCellGrid";
		this.GameOfLifeFixed.Add(this.GameOfLifeCellGrid);
		global::Gtk.Fixed.FixedChild w7 = ((global::Gtk.Fixed.FixedChild)(this.GameOfLifeFixed[this.GameOfLifeCellGrid]));
		w7.X = 73;
		w7.Y = 22;
		this.Add(this.GameOfLifeFixed);
		if ((this.Child != null))
		{
			this.Child.ShowAll();
		}
		this.DefaultWidth = 489;
		this.DefaultHeight = 475;
		this.Show();
		this.StartButton.Clicked += new global::System.EventHandler(this.OnStartButtonClicked);
		this.StopButton.Clicked += new global::System.EventHandler(this.OnStopButtonClicked);
		this.ResetButton.Clicked += new global::System.EventHandler(this.OnResetButtonClicked);
		this.QuitButton.Clicked += new global::System.EventHandler(this.OnQuitButtonClicked);
	}
}