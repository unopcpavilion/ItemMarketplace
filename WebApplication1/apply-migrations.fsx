#r "nuget: Microsoft.Data.SqlClient"
#r "nuget: FSharp.Data.SqlClient"

open Microsoft.Data.SqlClient

let connectionString =
    "data source=.;initial catalog=Marketplace;Encrypt=False;TrustServerCertificate=True;Integrated Security=true;MultipleActiveResultSets=True"
    
let createDatabaseIfNotExists (databaseName: string) =  
    let checkDatabaseExistsSql = $"SELECT * FROM sys.databases WHERE name='{databaseName}'"  
    let createDatabaseSql = $"CREATE DATABASE [{databaseName}]"  
  
    use connection = new SqlConnection("data source=.;Encrypt=False;TrustServerCertificate=True;Integrated Security=true;MultipleActiveResultSets=True")  
    connection.Open()  
    use checkCommand = new SqlCommand(checkDatabaseExistsSql, connection)  
    let result = checkCommand.ExecuteScalar()  
  
    if isNull result then  
        use createCommand = new SqlCommand(createDatabaseSql, connection)  
        createCommand.ExecuteNonQuery() |> ignore  
        printfn $"Database '{databaseName}' created successfully."  
    else  
        printfn $"Database '{databaseName}' already exists."  
  
let executeSql (connectionString: string) (sql: string) =
    use connection = new SqlConnection(connectionString)
    connection.Open()
    use command = new SqlCommand(sql, connection)
    command.ExecuteNonQuery() |> ignore

let applyMigration (migrationFile: string) =
    let sql = System.IO.File.ReadAllText(migrationFile)
    executeSql connectionString sql
    printfn $"Migration '{migrationFile}' applied successfully" 
    
createDatabaseIfNotExists "Marketplace"  
applyMigration "Migrations/001_InitialMigration.sql"
