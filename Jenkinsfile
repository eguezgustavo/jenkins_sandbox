def versionNumber = ""

pipeline {
  agent any 

  parameters {
    string(defaultValue: "1", description: 'Major version', name: 'MAJOR')
    string(defaultValue: "0", description: 'Minor version', name: 'MINOR')
    string(defaultValue: "0", description: 'Patch', name: 'PATCH')
  }

  stages {
    stage('Create Zip file') {
      steps {
        // sh './build.sh --build_version="${versionNumber}"'
        echo "hola mundo ${MAJOR}.${MINOR}.${PATCH}"
      }
    }
  }
}
