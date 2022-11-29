grammar FluentValidatorDSL;

compileUnit
	: block	EOF
	;

block
	: (statement)*
	;

statement
	: ruleFor
	;

ruleFor
	: RuleFor ID validatorList End
	;

exprList
	: expression (',' expression)*
	;

validatorList
	: validator (validator)*
	;

validator
	: ':' ID '(' exprList? ')'
	;

callFunction
	: ID '(' exprList? ')'
	;

expression
	: callFunction
	| NUMBER
	| BOOLEAN
	| STRING
	| ID
	;


fragment LETTER     : [a-zA-Z] ;
fragment DIGIT      : [0-9] ;
fragment A : [aA];
fragment B : [bB];
fragment C : [cC];
fragment D : [dD];
fragment E : [eE];
fragment F : [fF];
fragment G : [gG];
fragment H : [hH];
fragment I : [iI];
fragment J : [jJ];
fragment K : [kK];
fragment L : [lL];
fragment M : [mM];
fragment N : [nN];
fragment O : [oO];
fragment P : [pP];
fragment Q : [qQ];
fragment R : [rR];
fragment S : [sS];
fragment T : [tT];
fragment U : [uU];
fragment V : [vV];
fragment W : [wW];
fragment X : [xX];
fragment Y : [yY];
fragment Z : [zZ];

RuleFor	: R U L E F O R;
End : E N D;
And: A N D;
Or: O R;

ID 
	:	[a-zA-Z_] [a-zA-Z_0-9]* 
	;

NAME
	: LETTER+ 
	;

STRING
	: '\'' ( EscapeSequence | ~('\''|'\\') )* '\''
	;

NUMBER              
	: '-'? DIGIT+ ('.' DIGIT+)? 
	;

BOOLEAN
	:	'true'
	|	'false'
	;

EscapeSequence
	: '\\' [']
	;

Comment
	: ('//' ~[\r\n]* | '/*' .*? '*/') -> skip
	;

WHITESPACE          
	: [ \t\r\n\u000C] -> channel(HIDDEN)
	;
