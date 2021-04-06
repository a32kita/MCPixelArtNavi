﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Win32;

using Prism.Commands;
using Prism.Mvvm;

using MCPArtNavi.Common;
using MCPArtNavi.Common.PxartFileUtils;
using System.ComponentModel;

namespace MCPArtNavi.UserApp
{
    public class MainWindowViewModel : BindableBase
    {
        // 非公開フィールド


        // 公開プロパティ


        // バインディング プロパティ

        private PixelCanvasViewModel _canvasViewModel;

        public PixelCanvasViewModel CanvasViewModel
        {
            get => this._canvasViewModel;
            set => this.SetProperty(ref this._canvasViewModel, value);
        }

        private Visibility _canvasVisibility;

        public Visibility CanvasVisibility
        {
            get => this._canvasVisibility;
            set => this.SetProperty(ref this._canvasVisibility, value);
        }

        private Visibility _loadingTextVisibility;

        public Visibility LoadingTextVisibility
        {
            get => this._loadingTextVisibility;
            set => this.SetProperty(ref this._loadingTextVisibility, value);
        }

        private double _canvasZoom;

        public double CanvasZoom
        {
            get => this._canvasZoom;
            set => this.SetProperty(ref this._canvasZoom, value);
        }

        private bool _showChunkLinesChecked;

        public bool ShowChunkLinesChecked
        {
            get => this._showChunkLinesChecked;
            set => this.SetProperty(ref this._showChunkLinesChecked, value);
        }

        private Visibility _chunkLinesVisibility;

        public Visibility ChunkLinesVisibility
        {
            get => this._chunkLinesVisibility;
            set => this.SetProperty(ref this._chunkLinesVisibility, value);
        }


        // コマンド

        public DelegateCommand SaveAsCommand
        {
            get => new DelegateCommand(this._saveAs_command);
        }

        public DelegateCommand OpenCommand
        {
            get => new DelegateCommand(this._open_command);
        }

        public DelegateCommand LoadExampleImageCommand
        {
            get => new DelegateCommand(this._debug_loadExampleArt);
        }


        // コンストラクタ

        public MainWindowViewModel()
        {
            this.CanvasViewModel = new PixelCanvasViewModel();
            this.CanvasZoom = 4.0d;

            this.CanvasVisibility = Visibility.Visible;
            this.LoadingTextVisibility = Visibility.Collapsed;
            this.ShowChunkLinesChecked = true;
        }


        // 限定公開メソッド

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            // 基底呼び出し
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(ShowChunkLinesChecked):
                    if (this.ShowChunkLinesChecked)
                        this.ChunkLinesVisibility = Visibility.Visible;
                    else
                        this.ChunkLinesVisibility = Visibility.Hidden;
                    break;
            }
        }


        // 非公開メソッド

        private void _open_command()
        {
            var openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "MC Pixel Art Navi Document (*.mcpxart)|*.mcpxart|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                PixelArtDocument doc = null;
                using (var fs = File.OpenRead(openFileDialog.FileName))
                {
                    doc = PixelArtFile.LoadFrom(fs);
                }

                this.CanvasVisibility = Visibility.Hidden;
                this.LoadingTextVisibility = Visibility.Visible;
                this.CanvasViewModel.LoadPixelArt(doc);
                this.CanvasVisibility = Visibility.Visible;
                this.LoadingTextVisibility = Visibility.Collapsed;
            }
        }

        private void _saveAs_command()
        {
            var saveFileDialog = new SaveFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = "pixelart.mcpxart",
                Filter = "MC Pixel Art Navi Document (*.mcpxart)|*.mcpxart|All Files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var doc = this.CanvasViewModel.GetPixelArt();
                using (var fs = File.OpenWrite(saveFileDialog.FileName))
                {
                    PixelArtFile.SaveTo(fs, doc);
                }
            }
        }

        private void _debug_loadExampleArt()
        {
            // Example
            this.CanvasVisibility = Visibility.Hidden;
            this.LoadingTextVisibility = Visibility.Visible;

            var white_wool = new Common.Items.MCWhiteWool();
            var black_wool = new Common.Items.MCBlackWool();

            var pxartDoc = new PixelArtDocument();
            pxartDoc.Size = PixelArtSize.Size128x128;
            pxartDoc.Pixels = new IMCItem[pxartDoc.Size.GetWidth() * pxartDoc.Size.GetHeight()];
            for (var i = 0; i < pxartDoc.Pixels.Length; i++)
            {
                if (i % 3 == 0)
                    pxartDoc.Pixels[i] = white_wool;
                else
                    pxartDoc.Pixels[i] = black_wool;
            }

            this.CanvasViewModel.LoadPixelArt(pxartDoc);

            this.CanvasVisibility = Visibility.Visible;
            this.LoadingTextVisibility = Visibility.Collapsed;
        }
    }
}
