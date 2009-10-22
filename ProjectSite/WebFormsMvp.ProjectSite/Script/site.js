/// <reference path="jquery-1.3.2.js" />
/// <reference path="jquery.fancybox/jquery.easing.1.3.js" />
$(function() {
    $("#whyVideoEmbedded").append(
        $("<object width=\"640\" height=\"480\">" +
          "  <param value=\"true\" name=\"allowfullscreen\"/>" +
          "  <param value=\"always\" name=\"allowscriptaccess\"/>" +
          "  <param value=\"http://www.vimeo.com/moogaloop.swf?clip_id=7142823&amp;server=www.vimeo.com&amp;show_title=1&amp;show_byline=1&amp;show_portrait=0&amp;color=&amp;fullscreen=1&amp;autoplay=1\" name=\"movie\"/>" +
          "  <embed width=\"640\" height=\"480\" allowscriptaccess=\"always\" allowfullscreen=\"true\" type=\"application/x-shockwave-flash\" src=\"http://www.vimeo.com/moogaloop.swf?clip_id=7142823&amp;server=www.vimeo.com&amp;show_title=1&amp;show_byline=1&amp;show_portrait=0&amp;color=&amp;fullscreen=1&amp;autoplay=1\"/>" +
          "</object>")
          );
    $("#whyVideoLink")
        .attr("href", "#whyVideoEmbedded")
        .fancybox({
            "overlayShow": true,
            "frameWidth": 640,
            "frameHeight": 480
        });
});