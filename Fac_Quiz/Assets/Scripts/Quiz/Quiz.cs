using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    
    // 구조체 와 구조체 끼리 의존관계 ??..
    public class QuizQuestions
    {
        private List<String> questions=new List<String>();

        // 생성자 public 으로 설정함으로써 클래스 외부에서 인스턴스 생성 가능
      

        public void AddQuestions(List<String> a)
        {
            foreach (var ques in a)
            {
                questions.Add(ques);
            }
        }

        public void AddQuestion(string a)
        {
            questions.Add(a);
        }
        
    }
}