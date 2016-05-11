This is a project building on the .net cloud search repo here:

https://github.com/martin-magakian/Amazing-Cloud-Search

The AmazingCloudSearch project in this repo was downloaded from the 2013 API branch here:

https://github.com/martin-magakian/Amazing-Cloud-Search/tree/2013-API

The AmazingCloudSearch project was altered to get it working with a AWS Cloudsearch domain I created in May 2016.
Updates were made around CRUD and basic searches - mostly dealing with JSON conversion code and some manipulation
of post url strings to get them to work.


TO USE:

This code should work with test IMDB data loaded in a new AWS cloud domain setup following the getting started directions on aws:

https://aws.amazon.com/cloudsearch/getting-started/


In TestConsole project, copy app.config.template and rename it app.config.  Set your AWS cloud search domain URL (or key) in appsettings->AWSCloudSearchKey

Please note the value of the domain URL should not include "search-" or "doc-" at the beginning.

