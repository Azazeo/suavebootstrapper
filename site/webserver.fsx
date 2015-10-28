// --------------------------------------------------------------------------------------
// Start up Suave.io
// --------------------------------------------------------------------------------------

#r "../packages/FAKE/tools/FakeLib.dll"
#r "../packages/Suave/lib/net40/Suave.dll"

open Fake
open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Types
open System.Net

let serverConfig = 
    let port = getBuildParamOrDefault "port" "8083" |> Sockets.Port.Parse
    { defaultConfig with bindings = [ HttpBinding.mk HTTP IPAddress.Loopback port ] }

type BlogPost = {Title: string; Body: string}

let blogPosts = [| {Title = "First post"; Body = "This is the first post in this awesome blog!"};
                   {Title = "2nd post"; Body = "Second blog post"} |]

let webPart = 
    choose [
        path "/" >>= Files.browseFileHome "index.html"
        path "/get_latest_blog_posts" >>= (OK (JsonConvert.SerializeObject blogPosts)) >>= setMimeType "application/json"
        pathScan "/get_blog_post/%d" (fun (id) -> (OK (JsonConvert.SerializeObject blogPosts.[id])) >>= setMimeType "application/json")

        pathRegex "(.*)\.html" >>= Files.browseHome
        pathRegex "(.*)\.css" >>= Files.browseHome
        pathRegex "(.*)\.js" >>= Files.browseHome
        path "/store" >>= (OK "Store")
        path "/store/browse" >>= (OK "Store")
        path "/store/details" >>= (OK "Details")
        pathScan "/store/details/%s/%d" (fun (a, id) -> OK (sprintf "Artist: %s; Id: %d" a id))
    ]

startWebServer serverConfig webPart
