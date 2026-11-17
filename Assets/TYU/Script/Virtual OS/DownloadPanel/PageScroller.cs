using System.Collections.Generic;
using UnityEngine;

namespace TYU.Script.Virtual_OS.DownloadPanel
{
    public class PageScroller
    {
        private List<GameObject> _pages;
        private int _currentOpenedPage;
        public PageScroller(List<GameObject> pages, int openedPage)
        {
            _pages = pages;
            foreach (var page in _pages)
                page.SetActive(false);
            _currentOpenedPage = openedPage - 1;
            _pages[_currentOpenedPage].SetActive(true);
        }

        public void NextPage()
        {
            _pages[_currentOpenedPage].SetActive(false);
            _currentOpenedPage++;
            _pages[_currentOpenedPage].SetActive(true);
            
        }

        public void PastPage()
        {
            _pages[_currentOpenedPage].SetActive(false);
            _currentOpenedPage--;
            _pages[_currentOpenedPage].SetActive(true);
        }
    }
}