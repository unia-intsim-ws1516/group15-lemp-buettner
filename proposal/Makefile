TEX=xelatex

all: proposal

.PHONY proposal:
	${TEX} proposal.tex
	makeindex proposal.tex
	biber proposal
	${TEX} proposal.tex
	${TEX} proposal.tex
	-@${RM} *.aux
	-@${RM} *.log
	-@${RM} *.thm
	-@${RM} *.out

clean:
	-@${RM} proposal.pdf
	-@${RM} *.aux
	-@${RM} *.log
	-@${RM} *.thm
	-@${RM} *.out
	-@${RM} *.tex~
