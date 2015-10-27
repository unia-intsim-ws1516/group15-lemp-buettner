TEX=latexmk -xelatex

all: proposal

.PHONY proposal:
	${TEX} proposal.tex
	-@${RM} *.aux
	-@${RM} *.log
	-@${RM} *.thm
	-@${RM} *.out
	-@${RM} *.bbl
	-@${RM} *.bcf
	-@${RM} *.blg
	-@${RM} *.ilg
	-@${RM} *.ind
	-@${RM} *.run.xml
	-@${RM} *.fls
	-@${RM} *_latexmk

clean:
	-@${RM} proposal.pdf
	-@${RM} *.aux
	-@${RM} *.log
	-@${RM} *.thm
	-@${RM} *.out
	-@${RM} *.tex~
	-@${RM} *.bbl
	-@${RM} *.bcf
	-@${RM} *.blg
	-@${RM} *.ilg
	-@${RM} *.ind
	-@${RM} *.run.xml
	-@${RM} *.fls
	-@${RM} *_latexmk
