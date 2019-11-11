
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.VBox vbox1;

	private global::Gtk.FileChooserWidget torrentFileChooser;

	private global::Gtk.Table torrentFileDetailsTable;

	private global::Gtk.Entry commentEntry;

	private global::Gtk.Label commentLabel;

	private global::Gtk.Entry createdByEntry;

	private global::Gtk.Label createdByLabel;

	private global::Gtk.Entry creationDateEntry;

	private global::Gtk.Label creationDateLabel;

	private global::Gtk.Entry pieceLengthEntry;

	private global::Gtk.Label pieceLengthLabel;

	private global::Gtk.Entry trackerEntry;

	private global::Gtk.Label trackerMainLabel;

	private global::Gtk.Label trackersBackupLabel;

	private global::Gtk.Entry trackersEntry;

	private global::Gtk.ScrolledWindow GtkScrolledWindow2;

	private global::Gtk.TreeView filesListView;

	private global::Gtk.HBox downloadingHBox;

	private global::Gtk.ScrolledWindow peersScrollWIndow;

	private global::Gtk.TreeView peersListView;

	private global::Gtk.Table downloadDetailsTable;

	private global::Gtk.Entry downloadedEntry;

	private global::Gtk.Label downloadedLabel;

	private global::Gtk.Entry downloadingPeersEntry;

	private global::Gtk.Label downloadingPeersLabel;

	private global::Gtk.Entry infoHashEntry;

	private global::Gtk.Label infoHashLabel;

	private global::Gtk.Entry seedPeersEntry;

	private global::Gtk.Label seedPeersLabel;

	private global::Gtk.Entry uploadedEntry;

	private global::Gtk.Label uploadedLabel;

	private global::Gtk.Table tableDownloads;

	private global::Gtk.HButtonBox hbuttonbox1;

	private global::Gtk.Button downloadButton;

	private global::Gtk.Button pauseContinueButton;

	private global::Gtk.ProgressBar progressbarDownload;

	protected virtual void Build()
	{
		global::Stetic.Gui.Initialize(this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString("BitTorrent Client");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.torrentFileChooser = new global::Gtk.FileChooserWidget(((global::Gtk.FileChooserAction)(0)));
		this.torrentFileChooser.Name = "torrentFileChooser";
		this.vbox1.Add(this.torrentFileChooser);
		global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.torrentFileChooser]));
		w1.Position = 0;
		// Container child vbox1.Gtk.Box+BoxChild
		this.torrentFileDetailsTable = new global::Gtk.Table(((uint)(6)), ((uint)(2)), false);
		this.torrentFileDetailsTable.Name = "torrentFileDetailsTable";
		this.torrentFileDetailsTable.RowSpacing = ((uint)(6));
		this.torrentFileDetailsTable.ColumnSpacing = ((uint)(6));
		// Container child torrentFileDetailsTable.Gtk.Table+TableChild
		this.commentEntry = new global::Gtk.Entry();
		this.commentEntry.CanFocus = true;
		this.commentEntry.Name = "commentEntry";
		this.commentEntry.IsEditable = true;
		this.commentEntry.InvisibleChar = '•';
		this.torrentFileDetailsTable.Add(this.commentEntry);
		global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.torrentFileDetailsTable[this.commentEntry]));
		w2.TopAttach = ((uint)(2));
		w2.BottomAttach = ((uint)(3));
		w2.LeftAttach = ((uint)(1));
		w2.RightAttach = ((uint)(2));
		w2.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child torrentFileDetailsTable.Gtk.Table+TableChild
		this.commentLabel = new global::Gtk.Label();
		this.commentLabel.Name = "commentLabel";
		this.commentLabel.LabelProp = global::Mono.Unix.Catalog.GetString("Comment:");
		this.torrentFileDetailsTable.Add(this.commentLabel);
		global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.torrentFileDetailsTable[this.commentLabel]));
		w3.TopAttach = ((uint)(2));
		w3.BottomAttach = ((uint)(3));
		w3.XOptions = ((global::Gtk.AttachOptions)(4));
		w3.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child torrentFileDetailsTable.Gtk.Table+TableChild
		this.createdByEntry = new global::Gtk.Entry();
		this.createdByEntry.CanFocus = true;
		this.createdByEntry.Name = "createdByEntry";
		this.createdByEntry.IsEditable = true;
		this.createdByEntry.InvisibleChar = '•';
		this.torrentFileDetailsTable.Add(this.createdByEntry);
		global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.torrentFileDetailsTable[this.createdByEntry]));
		w4.TopAttach = ((uint)(3));
		w4.BottomAttach = ((uint)(4));
		w4.LeftAttach = ((uint)(1));
		w4.RightAttach = ((uint)(2));
		w4.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child torrentFileDetailsTable.Gtk.Table+TableChild
		this.createdByLabel = new global::Gtk.Label();
		this.createdByLabel.Name = "createdByLabel";
		this.createdByLabel.LabelProp = global::Mono.Unix.Catalog.GetString("Created By:");
		this.torrentFileDetailsTable.Add(this.createdByLabel);
		global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.torrentFileDetailsTable[this.createdByLabel]));
		w5.TopAttach = ((uint)(3));
		w5.BottomAttach = ((uint)(4));
		w5.XOptions = ((global::Gtk.AttachOptions)(4));
		w5.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child torrentFileDetailsTable.Gtk.Table+TableChild
		this.creationDateEntry = new global::Gtk.Entry();
		this.creationDateEntry.CanFocus = true;
		this.creationDateEntry.Name = "creationDateEntry";
		this.creationDateEntry.IsEditable = true;
		this.creationDateEntry.InvisibleChar = '•';
		this.torrentFileDetailsTable.Add(this.creationDateEntry);
		global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.torrentFileDetailsTable[this.creationDateEntry]));
		w6.TopAttach = ((uint)(4));
		w6.BottomAttach = ((uint)(5));
		w6.LeftAttach = ((uint)(1));
		w6.RightAttach = ((uint)(2));
		w6.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child torrentFileDetailsTable.Gtk.Table+TableChild
		this.creationDateLabel = new global::Gtk.Label();
		this.creationDateLabel.Name = "creationDateLabel";
		this.creationDateLabel.LabelProp = global::Mono.Unix.Catalog.GetString("Creation Date:");
		this.torrentFileDetailsTable.Add(this.creationDateLabel);
		global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.torrentFileDetailsTable[this.creationDateLabel]));
		w7.TopAttach = ((uint)(4));
		w7.BottomAttach = ((uint)(5));
		w7.XOptions = ((global::Gtk.AttachOptions)(4));
		w7.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child torrentFileDetailsTable.Gtk.Table+TableChild
		this.pieceLengthEntry = new global::Gtk.Entry();
		this.pieceLengthEntry.CanFocus = true;
		this.pieceLengthEntry.Name = "pieceLengthEntry";
		this.pieceLengthEntry.IsEditable = true;
		this.pieceLengthEntry.InvisibleChar = '•';
		this.torrentFileDetailsTable.Add(this.pieceLengthEntry);
		global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.torrentFileDetailsTable[this.pieceLengthEntry]));
		w8.TopAttach = ((uint)(5));
		w8.BottomAttach = ((uint)(6));
		w8.LeftAttach = ((uint)(1));
		w8.RightAttach = ((uint)(2));
		w8.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child torrentFileDetailsTable.Gtk.Table+TableChild
		this.pieceLengthLabel = new global::Gtk.Label();
		this.pieceLengthLabel.Name = "pieceLengthLabel";
		this.pieceLengthLabel.LabelProp = global::Mono.Unix.Catalog.GetString("Piece Length:");
		this.torrentFileDetailsTable.Add(this.pieceLengthLabel);
		global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.torrentFileDetailsTable[this.pieceLengthLabel]));
		w9.TopAttach = ((uint)(5));
		w9.BottomAttach = ((uint)(6));
		w9.XOptions = ((global::Gtk.AttachOptions)(4));
		w9.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child torrentFileDetailsTable.Gtk.Table+TableChild
		this.trackerEntry = new global::Gtk.Entry();
		this.trackerEntry.CanFocus = true;
		this.trackerEntry.Name = "trackerEntry";
		this.trackerEntry.IsEditable = true;
		this.trackerEntry.InvisibleChar = '•';
		this.torrentFileDetailsTable.Add(this.trackerEntry);
		global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.torrentFileDetailsTable[this.trackerEntry]));
		w10.LeftAttach = ((uint)(1));
		w10.RightAttach = ((uint)(2));
		w10.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child torrentFileDetailsTable.Gtk.Table+TableChild
		this.trackerMainLabel = new global::Gtk.Label();
		this.trackerMainLabel.Name = "trackerMainLabel";
		this.trackerMainLabel.LabelProp = global::Mono.Unix.Catalog.GetString("Tracker (main):");
		this.torrentFileDetailsTable.Add(this.trackerMainLabel);
		global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.torrentFileDetailsTable[this.trackerMainLabel]));
		w11.XOptions = ((global::Gtk.AttachOptions)(4));
		w11.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child torrentFileDetailsTable.Gtk.Table+TableChild
		this.trackersBackupLabel = new global::Gtk.Label();
		this.trackersBackupLabel.Name = "trackersBackupLabel";
		this.trackersBackupLabel.LabelProp = global::Mono.Unix.Catalog.GetString("Trackers (backup):");
		this.torrentFileDetailsTable.Add(this.trackersBackupLabel);
		global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.torrentFileDetailsTable[this.trackersBackupLabel]));
		w12.TopAttach = ((uint)(1));
		w12.BottomAttach = ((uint)(2));
		w12.XOptions = ((global::Gtk.AttachOptions)(4));
		w12.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child torrentFileDetailsTable.Gtk.Table+TableChild
		this.trackersEntry = new global::Gtk.Entry();
		this.trackersEntry.CanFocus = true;
		this.trackersEntry.Name = "trackersEntry";
		this.trackersEntry.IsEditable = true;
		this.trackersEntry.InvisibleChar = '•';
		this.torrentFileDetailsTable.Add(this.trackersEntry);
		global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.torrentFileDetailsTable[this.trackersEntry]));
		w13.TopAttach = ((uint)(1));
		w13.BottomAttach = ((uint)(2));
		w13.LeftAttach = ((uint)(1));
		w13.RightAttach = ((uint)(2));
		w13.YOptions = ((global::Gtk.AttachOptions)(4));
		this.vbox1.Add(this.torrentFileDetailsTable);
		global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.torrentFileDetailsTable]));
		w14.Position = 1;
		w14.Expand = false;
		w14.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.GtkScrolledWindow2 = new global::Gtk.ScrolledWindow();
		this.GtkScrolledWindow2.Name = "GtkScrolledWindow2";
		this.GtkScrolledWindow2.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow2.Gtk.Container+ContainerChild
		this.filesListView = new global::Gtk.TreeView();
		this.filesListView.CanFocus = true;
		this.filesListView.Name = "filesListView";
		this.filesListView.EnableSearch = false;
		this.GtkScrolledWindow2.Add(this.filesListView);
		this.vbox1.Add(this.GtkScrolledWindow2);
		global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.GtkScrolledWindow2]));
		w16.Position = 2;
		// Container child vbox1.Gtk.Box+BoxChild
		this.downloadingHBox = new global::Gtk.HBox();
		this.downloadingHBox.Name = "downloadingHBox";
		this.downloadingHBox.Spacing = 6;
		// Container child downloadingHBox.Gtk.Box+BoxChild
		this.peersScrollWIndow = new global::Gtk.ScrolledWindow();
		this.peersScrollWIndow.Name = "peersScrollWIndow";
		this.peersScrollWIndow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child peersScrollWIndow.Gtk.Container+ContainerChild
		this.peersListView = new global::Gtk.TreeView();
		this.peersListView.CanFocus = true;
		this.peersListView.Name = "peersListView";
		this.peersScrollWIndow.Add(this.peersListView);
		this.downloadingHBox.Add(this.peersScrollWIndow);
		global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.downloadingHBox[this.peersScrollWIndow]));
		w18.Position = 0;
		// Container child downloadingHBox.Gtk.Box+BoxChild
		this.downloadDetailsTable = new global::Gtk.Table(((uint)(2)), ((uint)(6)), false);
		this.downloadDetailsTable.Name = "downloadDetailsTable";
		this.downloadDetailsTable.RowSpacing = ((uint)(6));
		this.downloadDetailsTable.ColumnSpacing = ((uint)(6));
		// Container child downloadDetailsTable.Gtk.Table+TableChild
		this.downloadedEntry = new global::Gtk.Entry();
		this.downloadedEntry.CanFocus = true;
		this.downloadedEntry.Name = "downloadedEntry";
		this.downloadedEntry.IsEditable = true;
		this.downloadedEntry.InvisibleChar = '•';
		this.downloadDetailsTable.Add(this.downloadedEntry);
		global::Gtk.Table.TableChild w19 = ((global::Gtk.Table.TableChild)(this.downloadDetailsTable[this.downloadedEntry]));
		w19.LeftAttach = ((uint)(5));
		w19.RightAttach = ((uint)(6));
		w19.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child downloadDetailsTable.Gtk.Table+TableChild
		this.downloadedLabel = new global::Gtk.Label();
		this.downloadedLabel.Name = "downloadedLabel";
		this.downloadedLabel.LabelProp = global::Mono.Unix.Catalog.GetString("Total Downloaded:");
		this.downloadDetailsTable.Add(this.downloadedLabel);
		global::Gtk.Table.TableChild w20 = ((global::Gtk.Table.TableChild)(this.downloadDetailsTable[this.downloadedLabel]));
		w20.LeftAttach = ((uint)(4));
		w20.RightAttach = ((uint)(5));
		w20.XOptions = ((global::Gtk.AttachOptions)(4));
		w20.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child downloadDetailsTable.Gtk.Table+TableChild
		this.downloadingPeersEntry = new global::Gtk.Entry();
		this.downloadingPeersEntry.CanFocus = true;
		this.downloadingPeersEntry.Name = "downloadingPeersEntry";
		this.downloadingPeersEntry.IsEditable = true;
		this.downloadingPeersEntry.InvisibleChar = '•';
		this.downloadDetailsTable.Add(this.downloadingPeersEntry);
		global::Gtk.Table.TableChild w21 = ((global::Gtk.Table.TableChild)(this.downloadDetailsTable[this.downloadingPeersEntry]));
		w21.TopAttach = ((uint)(1));
		w21.BottomAttach = ((uint)(2));
		w21.LeftAttach = ((uint)(3));
		w21.RightAttach = ((uint)(4));
		w21.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child downloadDetailsTable.Gtk.Table+TableChild
		this.downloadingPeersLabel = new global::Gtk.Label();
		this.downloadingPeersLabel.Name = "downloadingPeersLabel";
		this.downloadingPeersLabel.LabelProp = global::Mono.Unix.Catalog.GetString("Downloading Peers:");
		this.downloadDetailsTable.Add(this.downloadingPeersLabel);
		global::Gtk.Table.TableChild w22 = ((global::Gtk.Table.TableChild)(this.downloadDetailsTable[this.downloadingPeersLabel]));
		w22.TopAttach = ((uint)(1));
		w22.BottomAttach = ((uint)(2));
		w22.LeftAttach = ((uint)(2));
		w22.RightAttach = ((uint)(3));
		w22.XOptions = ((global::Gtk.AttachOptions)(4));
		w22.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child downloadDetailsTable.Gtk.Table+TableChild
		this.infoHashEntry = new global::Gtk.Entry();
		this.infoHashEntry.CanFocus = true;
		this.infoHashEntry.Name = "infoHashEntry";
		this.infoHashEntry.IsEditable = true;
		this.infoHashEntry.InvisibleChar = '•';
		this.downloadDetailsTable.Add(this.infoHashEntry);
		global::Gtk.Table.TableChild w23 = ((global::Gtk.Table.TableChild)(this.downloadDetailsTable[this.infoHashEntry]));
		w23.LeftAttach = ((uint)(1));
		w23.RightAttach = ((uint)(2));
		w23.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child downloadDetailsTable.Gtk.Table+TableChild
		this.infoHashLabel = new global::Gtk.Label();
		this.infoHashLabel.Name = "infoHashLabel";
		this.infoHashLabel.LabelProp = global::Mono.Unix.Catalog.GetString("Info Hash:");
		this.downloadDetailsTable.Add(this.infoHashLabel);
		global::Gtk.Table.TableChild w24 = ((global::Gtk.Table.TableChild)(this.downloadDetailsTable[this.infoHashLabel]));
		w24.XOptions = ((global::Gtk.AttachOptions)(4));
		w24.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child downloadDetailsTable.Gtk.Table+TableChild
		this.seedPeersEntry = new global::Gtk.Entry();
		this.seedPeersEntry.CanFocus = true;
		this.seedPeersEntry.Name = "seedPeersEntry";
		this.seedPeersEntry.IsEditable = true;
		this.seedPeersEntry.InvisibleChar = '•';
		this.downloadDetailsTable.Add(this.seedPeersEntry);
		global::Gtk.Table.TableChild w25 = ((global::Gtk.Table.TableChild)(this.downloadDetailsTable[this.seedPeersEntry]));
		w25.LeftAttach = ((uint)(3));
		w25.RightAttach = ((uint)(4));
		w25.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child downloadDetailsTable.Gtk.Table+TableChild
		this.seedPeersLabel = new global::Gtk.Label();
		this.seedPeersLabel.Name = "seedPeersLabel";
		this.seedPeersLabel.LabelProp = global::Mono.Unix.Catalog.GetString("Seeding Peers:");
		this.downloadDetailsTable.Add(this.seedPeersLabel);
		global::Gtk.Table.TableChild w26 = ((global::Gtk.Table.TableChild)(this.downloadDetailsTable[this.seedPeersLabel]));
		w26.LeftAttach = ((uint)(2));
		w26.RightAttach = ((uint)(3));
		w26.XOptions = ((global::Gtk.AttachOptions)(4));
		w26.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child downloadDetailsTable.Gtk.Table+TableChild
		this.uploadedEntry = new global::Gtk.Entry();
		this.uploadedEntry.CanFocus = true;
		this.uploadedEntry.Name = "uploadedEntry";
		this.uploadedEntry.IsEditable = true;
		this.uploadedEntry.InvisibleChar = '•';
		this.downloadDetailsTable.Add(this.uploadedEntry);
		global::Gtk.Table.TableChild w27 = ((global::Gtk.Table.TableChild)(this.downloadDetailsTable[this.uploadedEntry]));
		w27.TopAttach = ((uint)(1));
		w27.BottomAttach = ((uint)(2));
		w27.LeftAttach = ((uint)(5));
		w27.RightAttach = ((uint)(6));
		w27.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child downloadDetailsTable.Gtk.Table+TableChild
		this.uploadedLabel = new global::Gtk.Label();
		this.uploadedLabel.Name = "uploadedLabel";
		this.uploadedLabel.LabelProp = global::Mono.Unix.Catalog.GetString("Total Uploaded:");
		this.downloadDetailsTable.Add(this.uploadedLabel);
		global::Gtk.Table.TableChild w28 = ((global::Gtk.Table.TableChild)(this.downloadDetailsTable[this.uploadedLabel]));
		w28.TopAttach = ((uint)(1));
		w28.BottomAttach = ((uint)(2));
		w28.LeftAttach = ((uint)(4));
		w28.RightAttach = ((uint)(5));
		w28.XOptions = ((global::Gtk.AttachOptions)(4));
		w28.YOptions = ((global::Gtk.AttachOptions)(4));
		this.downloadingHBox.Add(this.downloadDetailsTable);
		global::Gtk.Box.BoxChild w29 = ((global::Gtk.Box.BoxChild)(this.downloadingHBox[this.downloadDetailsTable]));
		w29.Position = 1;
		this.vbox1.Add(this.downloadingHBox);
		global::Gtk.Box.BoxChild w30 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.downloadingHBox]));
		w30.Position = 3;
		w30.Expand = false;
		w30.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.tableDownloads = new global::Gtk.Table(((uint)(1)), ((uint)(2)), false);
		this.tableDownloads.Name = "tableDownloads";
		this.tableDownloads.RowSpacing = ((uint)(6));
		this.tableDownloads.ColumnSpacing = ((uint)(6));
		// Container child tableDownloads.Gtk.Table+TableChild
		this.hbuttonbox1 = new global::Gtk.HButtonBox();
		this.hbuttonbox1.Name = "hbuttonbox1";
		this.hbuttonbox1.Homogeneous = true;
		this.hbuttonbox1.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(3));
		// Container child hbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
		this.downloadButton = new global::Gtk.Button();
		this.downloadButton.CanFocus = true;
		this.downloadButton.Name = "downloadButton";
		this.downloadButton.UseUnderline = true;
		this.downloadButton.Label = global::Mono.Unix.Catalog.GetString("Download");
		this.hbuttonbox1.Add(this.downloadButton);
		global::Gtk.ButtonBox.ButtonBoxChild w31 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox1[this.downloadButton]));
		w31.Expand = false;
		w31.Fill = false;
		// Container child hbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
		this.pauseContinueButton = new global::Gtk.Button();
		this.pauseContinueButton.CanFocus = true;
		this.pauseContinueButton.Name = "pauseContinueButton";
		this.pauseContinueButton.UseUnderline = true;
		this.pauseContinueButton.Label = global::Mono.Unix.Catalog.GetString("Pause");
		this.hbuttonbox1.Add(this.pauseContinueButton);
		global::Gtk.ButtonBox.ButtonBoxChild w32 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox1[this.pauseContinueButton]));
		w32.Position = 1;
		w32.Expand = false;
		w32.Fill = false;
		this.tableDownloads.Add(this.hbuttonbox1);
		global::Gtk.Table.TableChild w33 = ((global::Gtk.Table.TableChild)(this.tableDownloads[this.hbuttonbox1]));
		w33.XOptions = ((global::Gtk.AttachOptions)(4));
		w33.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child tableDownloads.Gtk.Table+TableChild
		this.progressbarDownload = new global::Gtk.ProgressBar();
		this.progressbarDownload.Name = "progressbarDownload";
		this.progressbarDownload.PulseStep = 0.01D;
		this.tableDownloads.Add(this.progressbarDownload);
		global::Gtk.Table.TableChild w34 = ((global::Gtk.Table.TableChild)(this.tableDownloads[this.progressbarDownload]));
		w34.LeftAttach = ((uint)(1));
		w34.RightAttach = ((uint)(2));
		w34.YOptions = ((global::Gtk.AttachOptions)(4));
		this.vbox1.Add(this.tableDownloads);
		global::Gtk.Box.BoxChild w35 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.tableDownloads]));
		w35.Position = 4;
		w35.Expand = false;
		w35.Fill = false;
		this.Add(this.vbox1);
		if ((this.Child != null))
		{
			this.Child.ShowAll();
		}
		this.DefaultWidth = 1117;
		this.DefaultHeight = 537;
		this.Show();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler(this.OnDeleteEvent);
		this.torrentFileChooser.FileActivated += new global::System.EventHandler(this.OnFilechooserwidget1FileActivated);
		this.downloadButton.Clicked += new global::System.EventHandler(this.OnDownloadButtonClicked);
		this.pauseContinueButton.Clicked += new global::System.EventHandler(this.OnPauseContinueButtonClicked);
	}
}
