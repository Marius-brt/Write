using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Documents;
using System.IO;
using Microsoft.Win32;

namespace Write
{
	public partial class MainWindow : Window
	{
		public string currentPath = "";
		private string filter = "Writer File (*.writer)|*.writer|Text (*.txt)|*.txt";

		#region Window Events

		public MainWindow()
		{
			InitializeComponent();
			this.FontComboBox.SelectionChanged += new SelectionChangedEventHandler(FontComboBoxChanged);
			this.FontFamilyComboBox.SelectionChanged += new SelectionChangedEventHandler(FontFamilyComboBoxChanged);
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
			{
				FontFamilyComboBox.Items.Add(fontFamily.Source);
			}
			FontFamilyComboBox.SelectedIndex = 0;
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (this.WindowState==WindowState.Maximized)
			{
				RestoreBnt.Visibility=Visibility.Visible;
				MaximizeBnt.Visibility=Visibility.Collapsed;
			}

			if(this.WindowState==WindowState.Normal)
			{
				RestoreBnt.Visibility=Visibility.Collapsed;MaximizeBnt.Visibility=Visibility.Visible;
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (SaveIndicator.Visibility == Visibility.Visible)
			{
				MessageBoxResult result = MessageBox.Show("Do you want to save ?", "Write", MessageBoxButton.YesNoCancel);
				switch (result)
				{
					case MessageBoxResult.Yes:
						Save();
						break;
					case MessageBoxResult.Cancel:
						e.Cancel =true;
						break;
				}
			}
		}

		#endregion

		#region Window Buttons

		#region Minimize Button

		private void MinimizeBnt_MouseEnter(object sender, MouseEventArgs e)
		{
			MinimizeBnt.Background=new SolidColorBrush(Color.FromArgb(60, 255, 255, 255));
		}

		private void MinimizeBnt_MouseLeave(object sender, MouseEventArgs e)
		{
			MinimizeBnt.Background=new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
		}

		private void MinimizeBnt_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.WindowState=WindowState.Minimized;
		}

		#endregion

		#region Restore Button

		private void RestoreBnt_MouseEnter(object sender, MouseEventArgs e)
		{
			RestoreBnt.Background=new SolidColorBrush(Color.FromArgb(60, 255, 255, 255));
		}

		private void RestoreBnt_MouseLeave(object sender, MouseEventArgs e)
		{
			RestoreBnt.Background=new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
		}

		private void RestoreBnt_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.WindowState=WindowState.Normal;
		}

		#endregion

		#region Maximize Button

		private void MaximizeBnt_MouseEnter(object sender, MouseEventArgs e)
		{
			MaximizeBnt.Background=new SolidColorBrush(Color.FromArgb(60, 255, 255, 255));
		}

		private void MaximizeBnt_MouseLeave(object sender, MouseEventArgs e)
		{
			MaximizeBnt.Background=new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
		}

		private void MaximizeBnt_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.WindowState=WindowState.Maximized;
		}

		#endregion

		#region Close Button

		private void CloseBnt_MouseEnter(object sender, MouseEventArgs e)
		{
			CloseBnt.Background=new SolidColorBrush(Color.FromArgb(60, 255, 255, 255));
		}

		private void CloseBnt_MouseLeave(object sender, MouseEventArgs e)
		{
			CloseBnt.Background=new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
		}

		private void CloseBnt_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Close();
		}

		#endregion

		private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		#endregion

		#region Menu Strip

		#region Buttons

		private void OpenBnt_Click(object sender, RoutedEventArgs e)
		{
			Open();
		}

		private void SaveBnt_Click(object sender, RoutedEventArgs e)
		{
			if (currentPath != string.Empty)
				Save();
			else
				SaveAs();
		}

		private void SaveAsBnt_Click(object sender, RoutedEventArgs e)
		{
			SaveAs();
		}

		private void NewFileBnt_Click(object sender, RoutedEventArgs e)
		{
			CreateNew();
		}

		private void MIClose_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		#endregion

		#region Commands

		private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Save();
		}

		private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			SaveAs();
		}

		private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CreateNew();
		}

		private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Open();
		}

		#endregion

		#endregion

		#region Tool Bar Buttons 

		#region Save Button

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			Save();
		}

		#endregion

		#region Text Alignement

		private void LeftBnt_Selected(object sender, RoutedEventArgs e)
		{
			ChangeTextProperty(Block.TextAlignmentProperty, "Left");
		}

		private void CenterBnt_Selected(object sender, RoutedEventArgs e)
		{
			ChangeTextProperty(Block.TextAlignmentProperty, "Center");
		}

		private void RightBnt_Selected(object sender, RoutedEventArgs e)
		{
			ChangeTextProperty(Block.TextAlignmentProperty, "Right");
		}

		private void JustifyBnt_Selected(object sender, RoutedEventArgs e)
		{
			ChangeTextProperty(Block.TextAlignmentProperty, "Justify");
		}

		#endregion

		#region Font Style

		#region Bold Button

		private void BoldBnt_Selected(object sender, RoutedEventArgs e)
		{
			ChangeTextProperty(TextElement.FontWeightProperty, "Bold");
		}

		private void BoldBnt_Unselected(object sender, RoutedEventArgs e)
		{
			ChangeTextProperty(TextElement.FontWeightProperty, "Normal");
		}

		#endregion

		#region Italic Button

		private void ItalicBnt_Selected(object sender, RoutedEventArgs e)
		{
			ChangeTextProperty(FontStyleProperty, "Italic");
		}

		private void ItalicBnt_Unselected(object sender, RoutedEventArgs e)
		{
			ChangeTextProperty(FontStyleProperty, "Normal");
		}

		#endregion

		#region Underline Button

		private void UnderlineBnt_Selected(object sender, RoutedEventArgs e)
		{
			ChangeTextProperty(Inline.TextDecorationsProperty, "Underline");
		}

		private void UnderlineBnt_Unselected(object sender, RoutedEventArgs e)
		{
			if (TextZone == null)
				return;
			TextSelection ts = TextZone.Selection;
			if (ts != null)
			{
				(ts.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection).TryRemove(TextDecorations.Underline, out TextDecorationCollection textDecorations);
				ts.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
			}
			TextZone.Focus();
		}

		#endregion

		private void FontComboBoxChanged(object sender, SelectionChangedEventArgs e)
		{
			ChangeTextProperty(RichTextBox.FontSizeProperty, (FontComboBox.SelectedItem as ComboBoxItem).Content.ToString());
		}

		private void FontFamilyComboBoxChanged(object sender, SelectionChangedEventArgs e)
		{
			TextSelection text = TextZone.Selection;
			string fontName = (string)FontFamilyComboBox.SelectedItem;
			if (!text.IsEmpty && fontName != null)
			{
				text.ApplyPropertyValue(RichTextBox.FontFamilyProperty, fontName);
			}
		}

		#endregion

		#region Speel Check Button

		private void SpellCheckBnt_Click(object sender, RoutedEventArgs e)
		{
			if (TextZone.SpellCheck.IsEnabled)
				TextZone.SpellCheck.IsEnabled = false;
			else
				TextZone.SpellCheck.IsEnabled = true;
		}

		#endregion

		#endregion

		#region Voids

		#region Text

		private void ChangeTextProperty(DependencyProperty dp, string value)
		{
			if (TextZone == null)
				return;
			TextSelection ts = TextZone.Selection;
			if (ts != null)
				ts.ApplyPropertyValue(dp, value);
			TextZone.Focus();
		}

		#endregion

		#region File

		private void CreateNew()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = filter };
			if (saveFileDialog.ShowDialog() == true)
			{
				if (currentPath == string.Empty)
				{
					Title = "Write / " + Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
					TextRange range;
					FileStream fStream;
					range = new TextRange(TextZone.Document.ContentStart, TextZone.Document.ContentEnd);
					fStream = new FileStream(saveFileDialog.FileName, FileMode.Create);
					range.Save(fStream, DataFormats.Rtf);
					fStream.Close();
					currentPath = saveFileDialog.FileName;
				}
				else
				{
					MainWindow newWindow = new MainWindow{Title = "Write / " + Path.GetFileNameWithoutExtension(saveFileDialog.FileName)};
					TextRange range;
					FileStream fStream;
					range = new TextRange(newWindow.TextZone.Document.ContentStart, newWindow.TextZone.Document.ContentEnd);
					fStream = new FileStream(saveFileDialog.FileName, FileMode.Create);
					range.Save(fStream, DataFormats.Rtf);
					fStream.Close();
					newWindow.currentPath = saveFileDialog.FileName;
					newWindow.Show();
				}
			}
		}

		private void Open()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog { Filter = filter};
			if (openFileDialog.ShowDialog() == true)
			{
				TextRange range;
				FileStream fStream;
				if (File.Exists(openFileDialog.FileName))
				{
					if (currentPath == string.Empty)
					{
						Title = "Write / " + Path.GetFileNameWithoutExtension(openFileDialog.FileName);
						currentPath = openFileDialog.FileName;
						range = new TextRange(TextZone.Document.ContentStart, TextZone.Document.ContentEnd);
						fStream = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate);
						if (new FileInfo(openFileDialog.FileName).Length != 0)
							range.Load(fStream, DataFormats.Rtf);
						else
							TextZone.Document.Blocks.Clear();
						fStream.Close();
					}
					else
					{
						MainWindow newWindow = new MainWindow{
						currentPath = openFileDialog.FileName,
						Title = "Write / " + Path.GetFileNameWithoutExtension(openFileDialog.FileName)};
						range = new TextRange(newWindow.TextZone.Document.ContentStart, newWindow.TextZone.Document.ContentEnd);
						fStream = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate);
						if (new FileInfo(openFileDialog.FileName).Length != 0)
							range.Load(fStream, DataFormats.Rtf);
						else
							TextZone.Document.Blocks.Clear();
						fStream.Close();
						newWindow.Show();
					}
				}
			}
		}

		private void Save()
		{
			if(currentPath != string.Empty && File.Exists(currentPath))
			{
				TextRange range;
				FileStream fStream;
				range = new TextRange(TextZone.Document.ContentStart, TextZone.Document.ContentEnd);
				fStream = new FileStream(currentPath, FileMode.Create);
				range.Save(fStream, DataFormats.Rtf);
				fStream.Close();
				SaveIndicator.Visibility = Visibility.Collapsed;
				SaveButton.IsEnabled = false;
			}
			else
			{
				SaveAs();
			}
		}

		private void SaveAs()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog{Filter = filter};
			if (saveFileDialog.ShowDialog() == true)
			{
				Title = "Write / " + Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
				TextRange range;
				FileStream fStream;
				range = new TextRange(TextZone.Document.ContentStart, TextZone.Document.ContentEnd);
				fStream = new FileStream(saveFileDialog.FileName, FileMode.Create);
				range.Save(fStream, DataFormats.Rtf);
				fStream.Close();
				SaveIndicator.Visibility = Visibility.Collapsed;
				SaveButton.IsEnabled = false;
				currentPath = saveFileDialog.FileName;
			}
		}

		#endregion

		#endregion

		#region Text Zone

		private void TextZone_SelectionChanged(object sender, RoutedEventArgs e)
		{
			if (TextZone == null)
				return;
			TextSelection ts = TextZone.Selection;
			if (ts != null)
			{
				if (ts.GetPropertyValue(FlowDocument.TextAlignmentProperty).Equals(TextAlignment.Left)) { LeftBnt.IsSelected = true; } else { LeftBnt.IsSelected = false; }
				if (ts.GetPropertyValue(FlowDocument.TextAlignmentProperty).Equals(TextAlignment.Center)) { CenterBnt.IsSelected = true; } else { CenterBnt.IsSelected = false; }
				if (ts.GetPropertyValue(FlowDocument.TextAlignmentProperty).Equals(TextAlignment.Right)) { RightBnt.IsSelected = true; } else { RightBnt.IsSelected = false; }
				if (ts.GetPropertyValue(FlowDocument.TextAlignmentProperty).Equals(TextAlignment.Justify)) { JustifyBnt.IsSelected = true; } else { JustifyBnt.IsSelected = false; }
				if (ts.GetPropertyValue(FontWeightProperty).ToString() == "Bold"){ BoldBnt.IsSelected = true; }else{ BoldBnt.IsSelected = false; }
				if (ts.GetPropertyValue(FontStyleProperty).ToString() == "Italic") { ItalicBnt.IsSelected = true; } else { ItalicBnt.IsSelected = false; }
				if (ts.GetPropertyValue(Inline.TextDecorationsProperty) == TextDecorations.Underline) { UnderlineBnt.IsSelected = true; } else { UnderlineBnt.IsSelected = false; }
				string selectedFont = ts.GetPropertyValue(FlowDocument.FontFamilyProperty).ToString();
				string selectedFontSize = ts.GetPropertyValue(FlowDocument.FontSizeProperty).ToString();
				for(int i = 0; i < FontFamilyComboBox.Items.Count; i++)
				{
					if(selectedFont.Contains(FontFamilyComboBox.Items[i].ToString()))
					{
						FontFamilyComboBox.SelectedIndex = i;
					}
				}
				for (int i = 0; i < FontComboBox.Items.Count; i++)
				{
					if (FontComboBox.Items[i].ToString().Contains(selectedFontSize))
					{
						FontComboBox.SelectedIndex = i;
					}
				}
			}
		}

		private void TextZone_TextChanged(object sender, TextChangedEventArgs e)
		{
			if(SaveIndicator != null)
				SaveIndicator.Visibility = Visibility.Visible;
			if (SaveButton != null)
				SaveButton.IsEnabled = true;
		}

		#endregion

	}
}