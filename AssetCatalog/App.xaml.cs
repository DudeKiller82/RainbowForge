﻿using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using AssetCatalog.Model;
using ModernWpf.Controls;
using RainbowForge;
using RainbowForge.Forge;

namespace AssetCatalog
{
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application, ICatalogApp, INotifyPropertyChanged
	{
		public static readonly ICatalogApp Catalog = (ICatalogApp) Current;

		private Forge _openedForge;

		private string _status;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public string Status
		{
			get => _status;
			set
			{
				_status = value;
				OnPropertyChanged();
			}
		}

		public Forge OpenedForge
		{
			get => _openedForge;
			private set
			{
				_openedForge = value;
				OnPropertyChanged();
			}
		}

		public void OpenForge(Stream stream)
		{
			var forgeStream = new BinaryReader(stream);
			OpenedForge = Forge.Read(forgeStream);
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class FileInstance
	{
		public Guid InstanceId { get; }
		public string Filename { get; }
		public AssetType AssetType { get; }

		public Page ContentPage { get; }

		public FileInstance(Guid instanceId)
		{
			InstanceId = instanceId;
		}
	}
}