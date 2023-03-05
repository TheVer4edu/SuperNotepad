# SuperNotepad

Tiny notepad app that may be used as example Model-View-ViewModel template.

This is a WPF Application, built in one Project in Solution. Project uses .NET of v7.0-windows, because WPF applications are unable to run on general version (v7.0). The -windows specification signs that this program may run only on machines with OS Windows (This is WPF limitation).

Firstly, project contains 4 directories (modules) and two files in root. This two files required to run the WPF App and were generated automatically with project template.

Keep mind on this three modules:

## View

This module contains in general two files:

 - MainWindow.xaml
 - MainWindow.xaml.cs

This is two files that contains one partial class named MainWindow. This class performs duties of View layer. 
The XAML file contains markup of the future window, which the user will see. 
Current XAML Markup contains initialization of DataContext that appears in the form of ViewModel, next is InputBindings with KeyBindings used to create shortcuts, 
and than the DockPanel layout, that contains all of UI Elements for user interaction. The DockPanel consists of 2 UI elements. Here is TextBox, that occupies most of the area of the DockPanel, before which situated the Menu.
Menu consists of few MenuItems that duplicates shortcut actions like "New", "Open", "Close" and "Save". By the way, TextBox is bound to ViewModel's Property `Contents`, that gots updates on every changes in this TextBox (It controlled by additional parameter `UpdateSourceTrigger=PropertyChanged`.

Second file is MainWindow.xaml.cs contains only initialization of View components.

## ViewModel

This module also contains two files. Here are:

 - ViewModel.cs
 - MainViewModel.cs

ViewModel is abstract class that implements interface INotifyPropertyChanged of standard system library. 
It passes only one event PropertyChanged, that being invoked when any property in this class changes theirs values. 
Moveover, here is two more methods: protected OnPropertyChanged that simply invokes previously described event and protected SetField method, 
that should be used by inheritors of this class. MainViewModel is concrete extension of ViewModel class for MainWindow, 
it contains one Property called Value, that updates own field value via super SetField method. 
Also here described a lot of inline-get ICommand Properties. 
They are bound on MenuItems and KeyBindings in MainWindow and called theirs action in case of some Button or Hotkey interacted. 
The Contents property is bound to TextBox UI element of MainWindow, and it instantly observes for updates and directly changes value of this property, 
being subscribed on previously described event PropertyChanged.

## Extra

In the module above, ICommand properties are initialized with special extra class called CommandDelegate.
This class implements System ICommand interface and allows to store its object to call execute method in case, when it required (eg. button clicked).

## Model

This module contains two more namespaces:

 - Data
 - Services

As you can see, Data contains only one FileDetails class, that fully describes full our data that we interacted, here only two Properties: FileName and FileContents. I guess, it's obvious why they are needed.
The next namespace contains Services, presented in the form of two classes: abstract FileService provides few most required methods, and LocalFileService, that implements all of its base class methods.

FileService is abstract class, bcz in theory, we may interact with files with some different ways (eg. Local file system, Network storage, Cloud storage and etc.). In our case we have only one implementation, that allows us to access the local file system.

In C# projects it's the better way to define Model layer in separate project of type "Class library", bcz in this way we possible to use the `internal` access modifier, that (for example) will restrict us to create instanses of Data Objects without service interaction.

You can use this code for any purpose however, the author does not guarantee its performance and correct operation. Licensed under the GNU General Public License v3.0
