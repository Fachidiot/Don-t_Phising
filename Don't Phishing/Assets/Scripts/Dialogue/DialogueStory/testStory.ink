-> start

=== start ===
눈을 떠보니 휴대폰에 이상한 메시지가 도착해 있었다.  

+ [메시지를 확인한다]
    -> suspicious_link

+ [그냥 무시하고 학교에 간다]
    -> go_to_school


=== suspicious_link ===
링크를 누르자, 은행 로고와 함께 로그인 페이지가 떴다.  

+ [로그인 정보를 입력한다]
    -> installed_phishing_app

+ [이상해서 URL을 다시 확인해본다]
    -> verify_link


=== installed_phishing_app ===
로그인 정보를 입력한 순간, 화면이 멈췄고 갑자기 이상한 앱 설치 화면이 떴다.  

#ENDING_BAD
-> END


=== verify_link ===
링크 주소를 다시 확인해보니 `bank-alerts.info.secure-login.co` 같은 수상한 도메인이었다.  
검색을 통해 이 링크가 피싱 사이트라는 것을 알아차렸다!

+ [앱을 닫고 경찰에 신고한다]
    -> report_police

+ [친구에게 공유해서 조심하라고 한다]
    -> warn_friends


=== report_police ===
경찰은 즉시 수사에 착수했고, 몇몇 피해자도 있었던 걸 알게 되었다.  

#ENDING_HERO
-> END


=== warn_friends ===
친구들이 고맙다며 조심하겠다고 한다.

#ENDING_GOOD
-> END


=== go_to_school ===
메시지를 무시하고 학교에 갔다.  

+ [그때 메시지를 다시 열어본다]
    -> suspicious_link

+ [지금은 이미 삭제해서 기억도 안 난다]
    -> forgotten_chance


=== forgotten_chance ===
기억도 안 나는 사이, 너의 계정은 이미 노출되었을지도 모른다.  
다음에는 주의하자... 
#ENDING_NEUTRAL
-> END
