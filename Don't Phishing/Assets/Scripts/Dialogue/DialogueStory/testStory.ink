-> start

=== start ===
눈을 뜨자마자 핸드폰 화면이 켜졌다. 알람이 와있다.

+ 알람을 확인한다
    -> check_alarm

+ 그냥 무시한다
    -> ignore_alarm


=== check_alarm ===
긴급: 당신의 계좌에서 이체가 발생했습니다.

+ 확인 링크를 눌러볼까?
    -> phishing_trap

+ 무시하고 은행 앱을 켠다
    -> safe_choice


=== ignore_alarm ===
너는 알람을 무시하고 다시 잠에 들었다.
#ENDING_SLEEP
-> END


=== phishing_trap ===
링크를 눌렀더니 이상한 앱이 깔렸다.
잠시 후, 핸드폰이 꺼졌다.
#ENDING_BAD
-> END


=== safe_choice ===
은행 앱을 열어보니 이상한 이체 내역이 있었다.
곧바로 신고해 계좌를 잠궜다.
#ENDING_GOOD
-> END
