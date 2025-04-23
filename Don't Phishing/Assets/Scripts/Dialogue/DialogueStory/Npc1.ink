-> start

=== start ===
#npc
^안녕? 혹시 나 기억 나려나 모르겠네..
^나 너 고등학교 때 같은 반이었는데.. 기억 못하려나?

+ [대답 하지 않는다]
    -> nodap

+ [대답 한다]
    -> yes

=== nodap ===
#npc
^아무 일도 벌어지지 않았다.
#ENDING_NEUTRAL
-> END

=== yes ===
#npc
^너 00고등학교 나왔잖아!
^2학년 때 3반이었고.
^서운한데? 나 기억 못해줘서


+ [답장한다]
    -> dapjang1

=== dapjang1 ===
#player
^어떻게 아시죠?? 아는 사람인가요?
-> npcNext

=== npcNext ===
#npc
^당연하지; 이 사진 기억나?
^나 이때는 좀 내성적이었어서..ㅎ
^아닌가?
^나 잘못 연락한 거 아니지? 설마 제가 잘못 연락했나요?

+ [답장한다]
    -> dapjang2

=== dapjang2 ===
#player
^아니 본인은 맞아요 저거 제 사진이라서.. 기억이 잘 안 나나 봐요
#ENDING_BAD
-> npcFinal

=== npcFinal ===
#npc
^뭐야 맞네
^어색하게 시리 반말 써 친구끼리
^너 설마 전화번호 바꿨어?
^카톡 프로필이 안 보여서
^너 번호 좀 알려 줘

+ [알려준다]
    -> endYes

+ [먼저 알려달라고 한다]
    -> endNo

=== endYes ===
#npc
^알고 보니, 나의 돈을 노린 스팸 문자였다!
#ENDING_BAD
-> END

=== endNo ===
#npc
^알고 보니, 나의 돈을 노린 스팸 문자였다!
#ENDING_BAD
-> END
