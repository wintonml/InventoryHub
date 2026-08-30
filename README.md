# InventoryHub

## Copilot contribution
Copilot helped in the following ways:
- Quickly generated the unit tests to verify the functionality of the objects
- Helped me identify an issue with the serialisation of the data. Before the
change the field names were being converted to camel case causing an issue with
processing the data. So, it helped me identify that I needed to change the
configuration of the serialiser.
- Helped with the try-catch statements for error handling and provide a smoother experience for the users.
- Made some suggestions for the structure of the project. For example, I asked if I should have the service interfaces and implementation in different folders, and it suggested that for smaller projects, it is unnecessary and leads to over-engineering. Suggested implementing this structure as the project grows.

Copilot was helpful in improving the performance of the app by helping with the caching of data. This helped reduce the number of calls to the backend. Then with the backend it helped with the caching strategies to help keep data fresh by implementing the expiration times of the cache.

Copilot was helpful in explaining parts of the code it generated so I would have a better understanding it. It was a collaborative process because if it implemented something in a way I did not like I would suggest a way it should be changed. This was a faster process as I did not need to write out the code myself.