Update IzendaSystemSetting
Set Value = 1
WHERE Name = 'DeploymentMode'


Update IzendaSystemSetting
Set Value = 'http://localhost:14810/api/izendaImplementation/validateIzendaAuthToken'
Where Name = 'AuthValidateAccessTokenUrl'


Update IzendaSystemSetting
Set Value = 'http://localhost:14810/api/izendaImplementation/GetIzendaAccessToken'
Where Name = 'AuthGetAccessTokenUrl'


Update IzendaSystemSetting
Set Value = 'http://localhost:81/'
Where Name = 'WebAPIUrl'

Update IzendaSystemSetting
Set Value = 'http://localhost:14810/' 
Where Name = 'WebUrl'


Update IzendaSystemSetting
Set Value = '<RSAKeyValue><Modulus>pCTo3pHhUtgLh5f4TV+IsD+l8/DuuFKu92j0w2ZHonndvCnAj/Ba3o1ts/crrbgmPZBLsUczH6G+XnLwWiJ6Lx0z9QIg6RcnWnBLHopq9WvUqN8YCisN+VxsaGS2uuOIANtEECSnJS74nmPGo69VIGJnSKJPJULJC/jz8mbPHA0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>'
Where Name = 'AuthRSAPublicKey'