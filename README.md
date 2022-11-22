# Trying to work out why it is slow for one labmda to invoke another


## Tools need to deploy
You will require .NET 6 and Node installed, then add the AWs Lambda Tools and AWS CDK (used to deploy the code)
```
dotnet tool install -g Amazon.Lambda.Tools
npm install -g aws-cdk
```

## Comands to deploy
Navigate to the folder containing the solution
```
# compile and package the lambdas
dotnet build
dotnet lambda package --project-location Catalog
dotnet lambda package --project-location Inventory

# use the cloud development kit to deploy the lambdas
cdk deploy --profile <your_profile>
```

## Trigger the Catalog lambda
Invoke the Catalog lambda in the console by passing in a string value to it. This then invokes the Inventory lambda with your string as the Payload, and it returns an Item object it news up.

## What we expected to see
Both lambdas would be cold so we expected a cold start for both. Lets say this cold start was half a second for both so the total time would be around 1 second. Subsequent calls soon after should take much less time.

## What we actually saw
The time taken for the first call was 6219.51 ms.
Looked in the CloudWatch and saw

Catalog: 

Duration: 6219.51 ms  Billed Duration: 6220 ms  Memory Size: 128 MB  Max Memory Used: 93 MB  Init Duration: 244.93 ms

Inventory:

Duration: 791.68 ms  Billed Duration: 792 ms  Memory Size: 128 MB  Max Memory Used: 61 MB  Init Duration: 224.84 ms

Subsequent calls to the Catalog lambda took around 100 ms total.

## The mystery is
Amount of time Catalog lambda executes = total time - inventory time - catalog init period

Amount of time Catalog lambda executes = 6219.51 ms - 791.68 ms - 244.93 ms

Amount of time Catalog lambda executes = 5182.9 ms

Why does it take over 5 seconds to invoke the Invetory lambda?