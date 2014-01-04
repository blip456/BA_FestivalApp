$(document).ready(function(){
	var win = $(window);
	var fxel = $('header');
	var eloffset = fxel.offset().top;
		console.log(win);
		console.log(fxel);
		console.log(eloffset);
	win.scroll(function()
	{
		if(eloffset < win.scrollTop())
		{
			fxel.addClass("fixed");
		}
		else
		{
			fxel.removeClass("fixed");
		}
	});
});
