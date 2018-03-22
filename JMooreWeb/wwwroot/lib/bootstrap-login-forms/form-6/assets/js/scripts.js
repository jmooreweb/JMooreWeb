
jQuery(document).ready(function() {
    
    /*
        Wow
    */
    new WOW().init();
    
    /*
	    Form validation
	*/
	$('.l-form-6-box input[type="text"], .l-form-6-box input[type="password"], .l-form-6-box textarea').on('focus', function() {
		$(this).removeClass('input-error');
	});
	
	$('.l-form-6-box form').on('submit', function(e) {
		
		$(this).find('input[type="text"], input[type="password"], textarea').each(function(){
			if( $(this).val() == "" ) {
				e.preventDefault();
				$(this).addClass('input-error');
			}
			else {
				$(this).removeClass('input-error');
			}
		});
		
	});
	
});
