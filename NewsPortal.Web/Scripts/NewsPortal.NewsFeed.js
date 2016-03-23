(function(NewsPortal) {
    (function (NewsFeed)
    {
        NewsFeed.OnLoad = function () {
            $("#NewsFeed").jscroll({
                padding: 20,
                nextSelector: "#nextpage"
            });
        }
    })(NewsPortal.NewsFeed || (NewsPortal.NewsFeed = {}));
})(window.NewsPortal || (window.NewsPortal = {}))
