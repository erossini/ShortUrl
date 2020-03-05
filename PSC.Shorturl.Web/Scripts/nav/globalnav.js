function create(htmlStr) {
    var frag = document.createDocumentFragment(),
        temp = document.createElement('div');

    temp.innerHTML = htmlStr;
    while (temp.firstChild) {
        frag.appendChild(temp.firstChild);
    }
    return frag;
}

// check if jQuery is loaded
if (!window.jQuery) {
    var jq = document.createElement('script'); jq.type = 'text/javascript';

    // Path to jquery.js file
    jq.src = 'http://psc.fyi/Scripts/nav/js/jquery.min.js';
    document.getElementsByTagName('head')[0].appendChild(jq);
}

var fragment = create('<div class="globalnav">' +
                      ' <div class="globalnav-sub">' +
                      '     <ul id="drop-nav">' +
                      '         <li>' +
                      '            <a href="#">PureSourceCode.com</a>' +
                      '             <div class="dropdown-main subs">' +
                      '                 <div class="dropdown-sub">' +
                      '                     <div class="left">' +
                      '                         <p class="mainheading">PureSourceCode.com</p>' +
                      '                         <p>' +
                      '                             My blogs about developing software and IT news' +
                      '                         </p>' +
                      '                     </div>' +
                      '                     <div class="right">' +
                      '                         <a href="http://www.puresourcecode.com/" target="_blank">Home Page <span>&#x25BA;</span></a>' +
                      '                         <a href="http://www.puresourcecode.com/dotnet" target="_blank">.NET <span>&#x25BA;</span></a>' +
                      '                         <a href="http://www.puresourcecode.com/news" target="_blank">News <span>&#x25BA;</span></a>' +
                      '                     </div>' +
                      '                 </div>' +
                      '             </div>' +
                      '         </li>' +
                      '         <li>' +
                      '            <a href="http://psc.fyi" target="_blank">PSC.FYI</a>' +
                      '         </li>' +
                      '         <li>' +
                      '           <a href="http://kindleitalia.net/" target="_blank">KindleItalia.net</a>' +
                      '         </li>' +
                      '         <li>' +
                      '            <a href="#">About me</a>' +
                      '             <div class="dropdown-main subs">' +
                      '                 <div class="dropdown-sub">' +
                      '                     <div class="left">' +
                      '                         <p class="mainheading">About me</p>' +
                      '                         <p>' +
                      '                             DO you want to know me or give me a job?' +
                      '                         </p>' +
                      '                         <p>' +
                      '                             My location is London, United Kingdom' +
                      '                         </p>' +
                      '                     </div>' +
                      '                     <div class="right">' +
                      '                         <a href="http://psc.fyi/cvpdf" target="_blank">CV in PDF format <span>&#x25BA;</span></a>' +
                      '                         <a href="http://psc.fyi/linkedin" target="_blank">LinkedIn <span>&#x25BA;</span></a>' +
                      '                         <a href="http://psc.fyi/twitter" target="_blank">Twitter <span>&#x25BA;</span></a>' +
                      '                     </div>' +
                      '                 </div>' +
                      '             </div>' +
                      '         </li>' +
                      '         <li class="righttext">' +
                      '             <div>' +
                      '                 <div class="locationname" id="locationname">Enrico Rossini</div>' +
                      '                 <img src="http://psc.fyi/Scripts/nav/images/location_icon.png"/>' +
                      '             </div>' +
                      '         </li>' +
                      '     </ul>' +
                      ' </div>' +
                      '</div>');

/* Add global nav into the page */
document.body.insertBefore(fragment, document.body.firstChild);

/* Add style for global navigation */
$('head').append('<link rel="stylesheet" type="text/css" href="http://psc.fyi/Scripts/nav/css/globalnav.css">');

/* Generate code for global nav */
var scriptcode = '$("#drop-nav > li > a").click(function (e) { ' +
				 '	if ($(this).next(".subs").length) { ' +
		         '    e.preventDefault();' +
				 '    }' +
			     '  if ($(this).parent().hasClass("selected")) {' +
				 '      $("#drop-nav .selected div.dropdown-main").slideUp(100); ' +
				 '      $("#drop-nav .selected").removeClass("selected"); ' +

			     '  } else { ' +
				 '      $("#drop-nav .selected div.dropdown-main").slideUp(100); ' +
				 '      $("#drop-nav .selected").removeClass("selected"); ' +
			     '      if ($(this).next(".subs").length) { ' +
				 '          e.preventDefault();' +
			     '          $(this).parent().addClass("selected"); ' +
			     '          $(this).next(".subs").slideDown(200);' +
			     '      } ' +
			     '  }' +
			     '  e.stopPropagation(); ' +
			     '}); ' +
			     '$("body").click(function () { ' +
                 '  $("#drop-nav .selected div.dropdown-main").slideUp(100);' +
                 '  $("#drop-nav .selected").removeClass("selected");' +
                 '});' +
                 'var x = location.hostname; ' +
                 'if(x.indexOf("psc") > -1) { document.getElementById("locationname").innerHTML = "Shorter URL"; } ' +
                 'if(x.indexOf("dotnet") > -1) { document.getElementById("locationname").innerHTML = "PureSourceCode > .NET"; }' +
                 'if(x.indexOf("kindleit")> -1) { document.getElementById("locationname").innerHTML= "KindleItalia.net"; } ',
                 head = document.head || document.getElementsByTagName('head')[0],
                 script = document.createElement('script');

/* Add the script into the tag script */
script.type = 'text/javascript';
script.appendChild(document.createTextNode(scriptcode));

/* Write the script in the page head */
head.appendChild(script);