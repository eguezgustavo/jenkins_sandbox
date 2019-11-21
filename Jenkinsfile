def versionNumber = ""
final LOCAL_FILE_NAME = "last-version.txt"
final REMOTE_FILE_NAME = "artifacts/" + BRANCH_NAME + "/" + LOCAL_FILE_NAME
final BUCKET_NAME = "jenkins-test-6756"

pipeline {
  agent any 

  parameters {
    string(description: 'Major version', name: 'MAJOR')
    string(description: 'Minor version', name: 'MINOR')
    string(description: 'Patch', name: 'PATCH')
  }

  stages {
    stage("Create Build Number") {
      steps {
        withAWS(credentials: "aws_only", region: "us-east-2") {
          script {

              jsonfile = readJSON(file: "pepe.json")
    					jsonfile["version"] = "a.a.a"
    					writeJSON(file: "pepe.json", json: jsonfile)

          }
        }
      }
    }
  }
}
