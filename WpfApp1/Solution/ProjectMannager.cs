﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace NLGSolution
{
    public class ProjectMannager : BaseObject
    {
        FileSystemWatcher watcher;

        public ProjectMannager(string ProjectPath):base(ProjectPath)
        {
            watcher = new FileSystemWatcher(ProjectPath);
            watcher.IncludeSubdirectories = false;
            watcher.Deleted += Watcher_Deleted;
            watcher.Created += Watcher_Created;
            watcher.Renamed += Watcher_Renamed;
            watcher.EnableRaisingEvents = true;
        }
        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            if (File.Exists(e.FullPath))
            {
                var projectFile = ProjectFiles.ToList().Find(t => t.FullPath == e.OldFullPath);
                projectFile.FullPath = e.FullPath;
                NotifyPropertyChanged("Children");

            }
            if (Directory.Exists(e.FullPath))
            {
                var projectFolder = ProjectFolders.ToList().Find(t => t.FullPath == e.OldFullPath);
                projectFolder.FullPath = e.FullPath;
                NotifyPropertyChanged("Children");
            }
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(e.FullPath))
            {
                var projectFile = ProjectFiles.ToList().Find(t => t.FullPath == e.FullPath);
                if (projectFile != null)
                {
                    ProjectFiles.Remove(projectFile);
                    NotifyPropertyChanged("Children");
                }
            }
            if (!Directory.Exists(e.FullPath))
            {
                var projectFolder = ProjectFolders.ToList().Find(t => t.FullPath == e.FullPath);
                if (projectFolder != null)
                {
                    ProjectFolders.Remove(projectFolder);
                    NotifyPropertyChanged("Children");
                }
            }
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (File.Exists(e.FullPath))
            {
                ProjectFile projectFile = new ProjectFile(e.FullPath)
                {
                    Name = e.Name,
                };
                AddChild(projectFile);
            }
            else if (Directory.Exists(e.FullPath))
            {
                ProjectFolder projectFolder = new ProjectFolder(e.FullPath)
                {
                    Name = e.Name,
                };
                AddChild(projectFolder);
            }

        }


        public Guid ConfirmationID { get; set; }

        public ObservableCollection<ProjectFolder> ProjectFolders { get; set; } = new ObservableCollection<ProjectFolder>();
        public ObservableCollection<ProjectFile> ProjectFiles { get; set; } = new ObservableCollection<ProjectFile>();
        public virtual void DoubleClick(MouseButtonEventArgs e)
        {
            MessageBox.Show("22222222222222222");
        }



        public ObservableCollection<object> Children
        {
            get
            {
                ObservableCollection<object> childNodes = new ObservableCollection<object>();
                foreach (var product in ProjectFolders)
                    childNodes.Add(product);
                foreach (var projectFile in ProjectFiles)
                    childNodes.Add(projectFile);
                return childNodes;
            }
        }
        public void AddChild(ProjectFolder projectFolder)
        {
            ProjectFolders.Add(projectFolder);

            NotifyPropertyChanged("Children");
        }
        public void AddChild(ProjectFile projectFile)
        {
            ProjectFiles.Add(projectFile);
            NotifyPropertyChanged("Children");
        }
        public void AddChilds(ProjectFile projectFile)
        {
            ProjectFiles.Add(projectFile);
        }
        public void AddChildsEnd()
        {
            NotifyPropertyChanged("Children");
        }
    }
}
