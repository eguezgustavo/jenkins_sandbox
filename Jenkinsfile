pipeline {
  agent any 
  stages {
    stage('Stage 1') {
      steps {
        sh 'echo "Stage 1 Step 1"'
      }
      steps {
        sh 'echo "Stage 1 Step 2"'
      }
    }
  }
  stages {
    stage('Stage 2') {
      steps {
        sh 'echo "Stage 2 Step 1"'
      }
      steps {
        sh 'echo "Stage 2 Step 2"'
      }
    }
  }
}
