-> start

=== start ===
눈을 떠보니 휴대폰에 이상한 메시지가 도착해 있었다.

+ [메시지를 확인한다]
    -> suspicious_link

+ [그냥 무시하고 학교에 간다]
    -> go_to_school


=== suspicious_link ===
링크를 누르자마자 이상한 앱 설치 화면이 떴다.

+ [설치한다]
    -> installed_phishing_app

+ [앱을 닫고 검색해본다]
    -> safe_research


=== installed_phishing_app ===
잠시 후, 휴대폰이 꺼졌고 은행에서 돈이 빠져나갔다.
#ENDING_BAD
-> END


=== safe_research ===
검색 결과, 피싱 앱이었다! 다행히도 설치 전에 막았다.
#ENDING_GOOD
-> END


=== go_to_school ===
메시지를 무시하고 학교에 도착했다. 평범한 하루가 시작되었다.
#ENDING_NEUTRAL
-> END
