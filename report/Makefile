OUTDIR=out
TEX=latexmk -xelatex -outdir=${OUTDIR}

all: report

.PHONY report:
	${TEX} report.tex
	cp ${OUTDIR}/report.pdf ./

clean:
	-@${RM} report.pdf
	-@${RM} -fr ${OUTDIR}
