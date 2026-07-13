
      (function(){
        try {
          if (localStorage.getItem('theme') === 'dark') {
            if (document.body) {
              document.body.classList.add('dark-mode');
            } else {
              // Ensure body gets the class immediately after it's parsed (before first paint)
              var obs = new MutationObserver(function(m, o){
                if (document.body) {
                  document.body.classList.add('dark-mode');
                  o.disconnect();
                }
              });
              obs.observe(document.documentElement, { childList: true });
            }
          }
        } catch(e) { /* silent */ }
      })();

